using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace EmisionService
{
    public class CryptoOperation
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(CryptoOperation));

        public string SignString(string keyBase64, string password, string cadenaOriginal)
        {
            try
            {
                var rsa = OpenKeyFile(keyBase64, password);
                if (rsa != null)
                {
                    var original = System.Text.Encoding.UTF8.GetBytes(cadenaOriginal);
                    var firma = rsa.SignData(original, new SHA256CryptoServiceProvider());
                    //var firma = rsa.SignData(original, "sha256");
                    return Convert.ToBase64String(firma);
                }
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                throw new Exception("No se genero la firma correctamente.");
            }
            string SignedString = "";

            
            return SignedString;
        }

        public RSACryptoServiceProvider OpenKeyFile(string keyBase64, string password)
        {
            
            var keyblob = Convert.FromBase64String(keyBase64);
            var identidad = new SecureString();// Se requerira un objeto SecureString que represente el password de la clave privada, que se obtiene asi:
            identidad.Clear();
            foreach (var c in password.ToCharArray())
            {
                identidad.AppendChar(c);
            }
            
            var rsa = DecodePrivateKeyInfo(keyblob, password); //PKCS #8 encrypted
            return rsa ?? null;
        }

        public static RSACryptoServiceProvider DecodePrivateKeyInfo(byte[] keyEncrip, string password)
        {
            byte[] dpkcs5Pbes2 = {0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x05, 0x0D};
            byte[] dpkcs5Pbkdf2 = {0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x05, 0x0C};
            byte[] ddesEde3Cbc = {0x06, 0x08, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x03, 0x07};
            var seqdes = new byte[10];
            var seq = new byte[11];
            int saltsize;
            int ivsize;
            int encblobsize;
            int iterations;

            // ---------  Set up stream to read the asn.1 encoded SubjectPublicKeyInfo blob  ------
            var mem = new MemoryStream(keyEncrip);
            //var lenstream = (int) mem.Length;
            var binr = new BinaryReader(mem); //wrap Memory Stream with BinaryReader for easy reading
            byte bt = 0;
            ushort twobytes = 0;

            try
            {

                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)
                    //data read as little endian order (actual data order for Sequence is 30 81)
                    binr.ReadByte(); //advance 1 byte
                else if (twobytes == 0x8230)
                    binr.ReadInt16(); //advance 2 bytes
                else
                    return null;

                twobytes = binr.ReadUInt16(); //inner sequence
                if (twobytes == 0x8130)
                    binr.ReadByte();
                else if (twobytes == 0x8230)
                    binr.ReadInt16();


                seq = binr.ReadBytes(11); //read the Sequence OID
                if (!CompareBytearrays(seq, dpkcs5Pbes2)) //is it a OIDpkcs5PBES2 ?
                    return null;

                twobytes = binr.ReadUInt16(); //inner sequence for pswd salt
                if (twobytes == 0x8130)
                    binr.ReadByte();
                else if (twobytes == 0x8230)
                    binr.ReadInt16();

                twobytes = binr.ReadUInt16(); //inner sequence for pswd salt
                if (twobytes == 0x8130)
                    binr.ReadByte();
                else if (twobytes == 0x8230)
                    binr.ReadInt16();

                seq = binr.ReadBytes(11); //read the Sequence OID
                if (!CompareBytearrays(seq, dpkcs5Pbkdf2)) //is it a OIDpkcs5PBKDF2 ?
                    return null;

                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)
                    binr.ReadByte();
                else if (twobytes == 0x8230)
                    binr.ReadInt16();

                bt = binr.ReadByte();
                if (bt != 0x04) //expect octet string for salt
                    return null;
                saltsize = binr.ReadByte();
                var salt = binr.ReadBytes(saltsize);

                bt = binr.ReadByte();
                if (bt != 0x02) //expect an integer for PBKF2 interation count
                    return null;

                int itbytes = binr.ReadByte(); //PBKD2 iterations should fit in 2 bytes.
                if (itbytes == 1)
                    iterations = binr.ReadByte();
                else if (itbytes == 2)
                    iterations = 256*binr.ReadByte() + binr.ReadByte();
                else
                    return null;

                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)
                    binr.ReadByte();
                else if (twobytes == 0x8230)
                    binr.ReadInt16();


                seqdes = binr.ReadBytes(10); //read the Sequence OID
                if (!CompareBytearrays(seqdes, ddesEde3Cbc)) //is it a OIDdes-EDE3-CBC ?
                    return null;

                bt = binr.ReadByte();
                if (bt != 0x04) //expect octet string for IV
                    return null;
                ivsize = binr.ReadByte(); // IV byte size should fit in one byte (24 expected for 3DES)
                var IV = binr.ReadBytes(ivsize);

                bt = binr.ReadByte();
                if (bt != 0x04) // expect octet string for encrypted PKCS8 data
                    return null;


                bt = binr.ReadByte();

                if (bt == 0x81)
                    encblobsize = binr.ReadByte(); // data size in next byte
                else if (bt == 0x82)
                    encblobsize = 256*binr.ReadByte() + binr.ReadByte();
                else
                    encblobsize = bt; // we already have the data size


                var encryptedpkcs8 = binr.ReadBytes(encblobsize);
                var secpswd = new SecureString();
                foreach (char c in password)
                    secpswd.AppendChar(c);

                var pkcs8 = DecryptPbdk2(encryptedpkcs8, salt, IV, secpswd, iterations);
                if (pkcs8 == null) // probably a bad pswd entered.
                    return null;

                var rsa = DecodePrivateKeyInfo(pkcs8);
                return rsa;
            }

            catch (Exception ee)
            {
                Logger.Error(ee);
                return null;
            }

            finally
            {
                binr.Close();
            }


        }

        private static bool CompareBytearrays(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;
            int i = 0;
            foreach (byte c in a)
            {
                if (c != b[i])
                    return false;
                i++;
            }
            return true;
        }

        public static byte[] DecryptPbdk2(byte[] edata, byte[] salt, byte[] IV, SecureString secpswd, int iterations)
        {
            CryptoStream decrypt = null;

            IntPtr unmanagedPswd = IntPtr.Zero;
            byte[] psbytes = new byte[secpswd.Length];
            unmanagedPswd = Marshal.SecureStringToGlobalAllocAnsi(secpswd);
            Marshal.Copy(unmanagedPswd, psbytes, 0, psbytes.Length);
            Marshal.ZeroFreeGlobalAllocAnsi(unmanagedPswd);

            try
            {
                Rfc2898DeriveBytes kd = new Rfc2898DeriveBytes(psbytes, salt, iterations);
                TripleDES decAlg = TripleDES.Create();
                decAlg.Key = kd.GetBytes(24);
                decAlg.IV = IV;
                MemoryStream memstr = new MemoryStream();
                decrypt = new CryptoStream(memstr, decAlg.CreateDecryptor(), CryptoStreamMode.Write);
                decrypt.Write(edata, 0, edata.Length);
                decrypt.Flush();
                decrypt.Close(); // this is REQUIRED.
                byte[] cleartext = memstr.ToArray();
                return cleartext;
            }
            catch (Exception e)
            {
                Console.WriteLine("Problem decrypting: {0}", e.Message);
                return null;
            }
        }

        public static RSACryptoServiceProvider DecodePrivateKeyInfo(byte[] pkcs8)
        {
            // encoded OID sequence for  PKCS #1 rsaEncryption szOID_RSA_RSA = "1.2.840.113549.1.1.1"
            // this byte[] includes the sequence byte and terminal encoded null 
            byte[] seqOid = {0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00};
            var seq = new byte[15];
            // ---------  Set up stream to read the asn.1 encoded SubjectPublicKeyInfo blob  ------
            var mem = new MemoryStream(pkcs8);
            var lenstream = (int) mem.Length;
            var binr = new BinaryReader(mem); //wrap Memory Stream with BinaryReader for easy reading
            byte bt = 0;
            ushort twobytes = 0;

            try
            {

                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                    binr.ReadByte(); //advance 1 byte
                else if (twobytes == 0x8230)
                    binr.ReadInt16(); //advance 2 bytes
                else
                    return null;


                bt = binr.ReadByte();
                if (bt != 0x02)
                    return null;

                twobytes = binr.ReadUInt16();

                if (twobytes != 0x0001)
                    return null;

                seq = binr.ReadBytes(15); //read the Sequence OID
                if (!CompareBytearrays(seq, seqOid)) //make sure Sequence for OID is correct
                    return null;

                bt = binr.ReadByte();
                if (bt != 0x04) //expect an Octet string 
                    return null;

                bt = binr.ReadByte(); //read next byte, or next 2 bytes is  0x81 or 0x82; otherwise bt is the byte count
                if (bt == 0x81)
                    binr.ReadByte();
                else if (bt == 0x82)
                    binr.ReadUInt16();
                //------ at this stage, the remaining sequence should be the RSA private key

                byte[] rsaprivkey = binr.ReadBytes((int) (lenstream - mem.Position));
                var rsacsp = DecodeRsaPrivateKey(rsaprivkey);
                return rsacsp;
            }

            catch (Exception ee)
            {
                Logger.Error(ee);
                return null;
            }

            finally
            {
                binr.Close();
            }
        }

        public static RSACryptoServiceProvider DecodeRsaPrivateKey(byte[] privkey)
        {
            byte[] modulus, e, d, p, q, dp, dq, iq;

            // ---------  Set up stream to decode the asn.1 encoded RSA private key  ------
            var mem = new MemoryStream(privkey);
            var binr = new BinaryReader(mem); //wrap Memory Stream with BinaryReader for easy reading
            byte bt = 0;
            ushort twobytes = 0;
            int elems = 0;
            try
            {
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                    binr.ReadByte(); //advance 1 byte
                else if (twobytes == 0x8230)
                    binr.ReadInt16(); //advance 2 bytes
                else
                    return null;

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102) //version number
                    return null;
                bt = binr.ReadByte();
                if (bt != 0x00)
                    return null;


                //------  all private key components are Integer sequences ----
                elems = GetIntegerSize(binr);
                modulus = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                e = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                d = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                p = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                q = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                dp = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                dq = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                iq = binr.ReadBytes(elems);

                Console.WriteLine("showing components ..");

                // ------- create RSACryptoServiceProvider instance and initialize with public key -----
                var rsa = new RSACryptoServiceProvider();
                var rsaParameters = new RSAParameters();
                rsaParameters.Modulus = modulus;
                rsaParameters.Exponent = e;
                rsaParameters.D = d;
                rsaParameters.P = p;
                rsaParameters.Q = q;
                rsaParameters.DP = dp;
                rsaParameters.DQ = dq;
                rsaParameters.InverseQ = iq;
                rsa.ImportParameters(rsaParameters);
                return rsa;
            }
            catch (Exception ee)
            {
                Logger.Error(ee);
                return null;
            }
            finally
            {
                binr.Close();
            }
        }

        private static int GetIntegerSize(BinaryReader binr)
        {
            int count = 0;
            var bt = binr.ReadByte();
            if (bt != 0x02) //expect integer
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte(); // data size in next byte
            else if (bt == 0x82)
            {
                var highbyte = binr.ReadByte(); // data size in next 2 bytes
                var lowbyte = binr.ReadByte();
                byte[] modint = {lowbyte, highbyte, 0x00, 0x00};
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt; // we already have the data size
            }
            while (binr.ReadByte() == 0x00)
            {
                //remove high order zeros in data
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);
            //last ReadByte wasn't a removed zero, so back up a byte
            return count;
        }
    }
}

