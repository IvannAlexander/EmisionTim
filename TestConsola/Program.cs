using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bussines;
using EmisionService;
using System.Xml.Linq;

namespace TestConsola
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                
                //var pwd = CommonOperation.Encrypt("alexander");
                GenerarPdf();
                ConvertirTxt();
                //var pwd = OperacionesComun.Encrypt("12345678a");
                //pwd = OperacionesComun.Decrypt(pwd);
                //pwd.ToString();

                var operacionesEmision = new EmisionOperation();

                var emision = new SerializationOperation();
                var xml = File.ReadAllText(@"D:\Data\_OUT\2018\20180406\AAA010101AAA_MAS980812UK1_JOSE_7442674.xml");
                var comprobante = emision.Deserializar<Comprobante>(xml);

                var xmlNom = File.ReadAllText(@"D:\Data\_OUT\2018\20180313\Nomina.xml");
                var nomina = emision.Deserializar<Nomina12Cfdi.Nomina12>(xmlNom);
                comprobante.ExisteComplemento = true;
                comprobante.Nomina12 = nomina;
                comprobante.TipoComplemento = "Nomina12";
                //var cfdiNom = emision.Serializar(nomina);
                //cfdiNom.ToString();
                
                //var cfdi = emision.Serializar(comprobante);
                //cfdi.ToString();

                
                //var cfdi = operacionesEmision.GeneraCfdi33(comprobante);
                //var cadena = operacionesEmision.ObtenerCadenaOriginal(cfdi);
                
                //cfdi.ToString();


                ///Svar datos del certificado en base 64
                ///var o = Convert.FromBase64String(certificadoBase64);
                ///X509Certificate2 _MiCertificado = new X509Certificate2(o, passCert);
                
                 
            }
            catch (Exception ee)
            {
                throw new Exception(ee.Message);
            }
        }

        private static void GenerarPdf()
        {
            try
            {
                var operacionesPdf = new PdfOperation();

                var cfdi = File.ReadAllText(@"C:\Users\ivann\Downloads\new 3.xml");

                //var cfdi = File.ReadAllText(@"D:\Descargas\PMS010531UG0_XAXX010101000_UNIA_10.XML");
                //XDocument xml = XDocument.Parse(cfdi);
                var resp = operacionesPdf.GenerarPdfFactura(cfdi);
                File.WriteAllBytes(@"D:\Eliminar\PdfTest1111.pdf", resp);
            }
            catch (Exception ee)
            {

                throw new Exception(ee.Message);
            }
        }

            private static void ConvertirTxt()
        {
            try
            {
                var contenido = File.ReadAllText(@"D:\Alexander\Mega\Trabajo\edocuentacliente2(0000007181).txt");
                var convertirTxt = new EmisionService.ConvertTxtOperation();
                var comprobante = convertirTxt.ConvertirTxt(contenido);
                var operacionesEmision = new EmisionOperation();
                var noCertificado = "30001000000400002434";
                var certificado = "MIIFuzCCA6OgAwIBAgIUMzAwMDEwMDAwMDA0MDAwMDI0MzQwDQYJKoZIhvcNAQELBQAwggErMQ8wDQYDVQQDDAZBQyBVQVQxLjAsBgNVBAoMJVNFUlZJQ0lPIERFIEFETUlOSVNUUkFDSU9OIFRSSUJVVEFSSUExGjAYBgNVBAsMEVNBVC1JRVMgQXV0aG9yaXR5MSgwJgYJKoZIhvcNAQkBFhlvc2Nhci5tYXJ0aW5lekBzYXQuZ29iLm14MR0wGwYDVQQJDBQzcmEgY2VycmFkYSBkZSBjYWRpejEOMAwGA1UEEQwFMDYzNzAxCzAJBgNVBAYTAk1YMRkwFwYDVQQIDBBDSVVEQUQgREUgTUVYSUNPMREwDwYDVQQHDAhDT1lPQUNBTjERMA8GA1UELRMIMi41LjQuNDUxJTAjBgkqhkiG9w0BCQITFnJlc3BvbnNhYmxlOiBBQ0RNQS1TQVQwHhcNMTkwNjE3MTk0NDE0WhcNMjMwNjE3MTk0NDE0WjCB4jEnMCUGA1UEAxMeRVNDVUVMQSBLRU1QRVIgVVJHQVRFIFNBIERFIENWMScwJQYDVQQpEx5FU0NVRUxBIEtFTVBFUiBVUkdBVEUgU0EgREUgQ1YxJzAlBgNVBAoTHkVTQ1VFTEEgS0VNUEVSIFVSR0FURSBTQSBERSBDVjElMCMGA1UELRMcRUtVOTAwMzE3M0M5IC8gWElRQjg5MTExNlFFNDEeMBwGA1UEBRMVIC8gWElRQjg5MTExNk1HUk1aUjA1MR4wHAYDVQQLExVFc2N1ZWxhIEtlbXBlciBVcmdhdGUwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQCN0peKpgfOL75iYRv1fqq+oVYsLPVUR/GibYmGKc9InHFy5lYF6OTYjnIIvmkOdRobbGlCUxORX/tLsl8Ya9gm6Yo7hHnODRBIDup3GISFzB/96R9K/MzYQOcscMIoBDARaycnLvy7FlMvO7/rlVnsSARxZRO8Kz8Zkksj2zpeYpjZIya/369+oGqQk1cTRkHo59JvJ4Tfbk/3iIyf4H/Ini9nBe9cYWo0MnKob7DDt/vsdi5tA8mMtA953LapNyCZIDCRQQlUGNgDqY9/8F5mUvVgkcczsIgGdvf9vMQPSf3jjCiKj7j6ucxl1+FwJWmbvgNmiaUR/0q4m2rm78lFAgMBAAGjHTAbMAwGA1UdEwEB/wQCMAAwCwYDVR0PBAQDAgbAMA0GCSqGSIb3DQEBCwUAA4ICAQBcpj1TjT4jiinIujIdAlFzE6kRwYJCnDG08zSp4kSnShjxADGEXH2chehKMV0FY7c4njA5eDGdA/G2OCTPvF5rpeCZP5Dw504RZkYDl2suRz+wa1sNBVpbnBJEK0fQcN3IftBwsgNFdFhUtCyw3lus1SSJbPxjLHS6FcZZ51YSeIfcNXOAuTqdimusaXq15GrSrCOkM6n2jfj2sMJYM2HXaXJ6rGTEgYmhYdwxWtil6RfZB+fGQ/H9I9WLnl4KTZUS6C9+NLHh4FPDhSk19fpS2S/56aqgFoGAkXAYt9Fy5ECaPcULIfJ1DEbsXKyRdCv3JY89+0MNkOdaDnsemS2o5Gl08zI4iYtt3L40gAZ60NPh31kVLnYNsmvfNxYyKp+AeJtDHyW9w7ftM0Hoi+BuRmcAQSKFV3pk8j51la+jrRBrAUv8blbRcQ5BiZUwJzHFEKIwTsRGoRyEx96sNnB03n6GTwjIGz92SmLdNl95r9rkvp+2m4S6q1lPuXaFg7DGBrXWC8iyqeWE2iobdwIIuXPTMVqQb12m1dAkJVRO5NdHnP/MpqOvOgLqoZBNHGyBg4Gqm4sCJHCxA1c8Elfa2RQTCk0tAzllL4vOnI1GHkGJn65xokGsaU4B4D36xh7eWrfj4/pgWHmtoDAYa8wzSwo2GVCZOs+mtEgOQB91/g==";
                //var firma = operacionesCrypto.SignString("MIIFDjBABgkqhkiG9w0BBQ0wMzAbBgkqhkiG9w0BBQwwDgQIAgEAAoIBAQACAggAMBQGCCqGSIb3DQMHBAgwggS+AgEAMASCBMh4EHl7aNSCaMDA1VlRoXCZ5UUmqErAbuck7ujDnmKxSaOGzJzn1hAlfBWJNtr1rgiCXRHB5/2qJ/CnTOkCcgutvs1xl3vxHgY1+N9I60iZUG+yjfEd+ungL4alXXMtKgZ8CkQXaeYIeQXFdyZ5jUU07Cy+LjMrIOAh1m/VnL6U/qW3dY+oJmII6gCG0SKcfCojeCpBVL2ispK2CBTpMDO4hd7vnbFhafl9/wUkAncmz5SHLjXPMKgmK7HvBiUSMRYFCjcNEBvMshI7E1//nG8pi0Xrmbq4MfT1B+SF8vbA39hCqKP32m+QFlOduHlaFSnW96UkMBT5hF1qImwU3HTbtKfAumo3BLzYJ9XP7Y6eVOFFSSsXudrAt94mH7CojUjazGHBsqagsUY85Q7Cz0TTvnnvWFNFAj/xbQm6nT1VL8FkdJm8hEb5YLaOqQZ8y1AEv8sCq/M51aHglexuzGFIIUTF+/XQGeYDBITlS6z2TryoHp8n1+6LpClL51WrIfaSxyMEtG2fmAHN82iNujOP6MBR7aMZ6dfxJctFRAaWlmi89wa5VhyeaoDzkx1roJznF3MLxVKROmYLDYk142IwRtTgWrex4Wnidpo4unrfL+uj6VwTUDk0cizaYvamRhlZ/LXdwyB1syb/GQGu94gSzB1zAzb5/IIbtyofK+/tVTv08OMpCqHfBye1QJQg+vxQHMkbhZH6sEgORSEjuidW13DTKi8xyryQsD5WccMh8WDxMuAVFUldrwWdGilFKg0G99S/XJWLwKB74Nv0v/Ygdp6/d9T0fFD3FXpb9RznErCgfVSMtrPv1svGg3QFwh+qmkzjh+NBwUrmmqEjNshji+9SB4fnJNYlKVvu4WAzMKliixUkRcCID1QYwLtiyWuwZDYxKTnk7Y1LXmRGqqhNbh4kdnTNUdkxEjqp+UVtBdaxswa6s4qrLbNeD7VN+1KJEMN6/zZ6+2Uj2KBMzaDA0zwAHMB1gyPkgX0v47e61iffaVAUQzDGYGbDERG2vT8234NSDdgqzOpsf7il2Pv+uF0oab+db62JiRvOEjNefXG5p8KRudYyaVO8N7iTdRRj/A/yDwjmSq9dDCZEZE0cD6BEaAgmjvqwF5IvGgJGnWYKhrOGBPv+VL6zGOXo/L9zenxYwKNTHzNYlvug/t4gXQmArroqA2YKBGpYb8/FY/q3t3k+u1bXWvNLOzWi81InvxFSCTu6l9GBCthyWwekWdoL6ssSzOmzPr/d2klSRST3ByJmAJzLGJFsj6AL01BaUVWERH0s+GmnSWOU8ZIQVGF7aOEWWbtD0vyjJRxQnxPxn+Tt3oT9Nob10QGwG/2tNZtZuhAMf1yt+cF8jl0hC/LI0FtMqmLAkxaEOiXHmFuKXbAjFxIjdIwgWsAZe1cTLzR44jIKwlB64jvh1LXmJ5jCszLd/fuCEB0XZUWLDRCZVb82MqcZl7U/gaFazSqm71NNafCDzWjWO4ukWN1lcTDJE05KgeRqoYIEcpU8jXy/CAEaoseA1bWDnfnLJk2axVXzmrtYnojyKjTjDz3In41Kjsx3nNOegqtH7O2gl9YBzJfgwZmF0ldk+udcotc0JwIXYWk7b5HmRgXWa+WvDHSwyLzMrbw=", "12345678a", cadenaOriginal);
                var key = "MIIFDjBABgkqhkiG9w0BBQ0wMzAbBgkqhkiG9w0BBQwwDgQIAgEAAoIBAQACAggAMBQGCCqGSIb3DQMHBAgwggS8AgEAMASCBMh4EHl7aNSCaMDA1VlRoXCZ5UUmqErAbucRFLOMmsAaFNkyWR0dXIAh0CMjE6NpQIMZhQ0HH/4tHgmwh4kCawGjIwERoG6/IH3mCt7u19J5+m6gUEGOJdEMXj976E5lKCd/EG6t6lCq66GE3rgux/nFmeQZvsjLlzPyhe2j+X81LrGudITTjDdgLI0EdbdV9CUJwWbibzrVxjuAVShRh07XPL/DiEw3Wk2+kdy4cfWmMvh0U55p0RKZopNkWuVVSvr3ai7ZNCwHZWDVqkUDpwDDGdyt0kYQ7qoKanIxv/A9wv6ekq0LQ/yLlOcelkxQeb8Glu4RXe+krRvrASw1eBAQ3mvNKpngwF8vtlyoil41PjHUOKALMJtNpywckRRYOk4703ylWIzTfdBlrZ6VmDBjdC5723G1HAx3R/x+o+08++RNiFaN06Ly5QbZZvjnealDfSKz1VKRHWeXggaW87rl4n0SOOWnvabKs4ZWRXTS0dhWK+KD/yYYQypTslDSXQrmyMkpc1Zcb4p9RTjodXxGCWdsR5i5+Ro/RiJvxWwwaO3YW6eaSavV0ROqANQ+A+GizMlxsVjl6G5Ooh6ORdA7jTNWmK44Icgyz6QFNh+J3NibxVK2GZxsQRi+N3HXeKYtq5SDXARA0BsaJQzYfDotA9LFgmFKg9jVhtcc1V3rtpaJ5sab8tdBTPPyN/XT8fA0GxlIX+hjLd3E9wB7qzNR6PZ84UKDxhCGWrLuIoSzuCbr+TD9UCJprsfTu8kr8Pur4rrxm7Zu1MsJRR9U5Ut+O9FZfw4SqGykyTGGh0v1gDG8esKpTW5MKNk9dRwDNHEmIF6tE6NeXDlzovf8VW6z9JA6AVUkgiFjDvLUY5MgyTqPB9RJNMSAZBzrkZgXyHlmFz2rvPqQGFbAtukjeRNS+nkVayLqfQnqpgthBvsgDUgFn03z0U2Svb094Q5XHMeQ4KM/nMWTEUC+8cybYhwVklJU7FBl9nzs66wkMZpViIrVWwSB2k9R1r/ZQcmeL+LR+WwgCtRs4It1rNVkxXwYHjsFM2Ce46TWhbVMF/h7Ap4lOTS15EHC8RvIBBcR2w1iJ+3pXiMeihArTELVnQsS31X3kxbBp3dGvLvW7PxDlwwdUQOXnMoimUCI/h0uPdSRULPAQHgSp9+TwqI0Uswb7cEiXnN8PySN5Tk109CYJjKqCxtuXu+oOeQV2I/0knQLd2zol+yIzNLj5a/HvyN+kOhIGi6TrFThuiVbbtnTtRM1CzKtFGuw5lYrwskkkvenoSLNY0N85QCU8ugjc3Bw4JZ9jNrDUaJ1Vb5/+1GQx/q/Dbxnl+FK6wMLjXy5JdFDeQyjBEBqndQxrs9cM5xBnl6AYs2Xymydafm2qK0cEDzwOPMpVcKU8sXS/AHvtgsn+rjMzW0wrQblWE0Ht/74GgfCj4diCDtzxQ0ggi6yJD+yhLZtVVqmKS3Gwnj9RxPLNfpgzPP01eYyBBi/W0RWTzcTb8iMxWX52MTU0oX9//4I7CAPXn0ZhpWAAIvUmkfjwfEModH7iwwaNtZFlT2rlzeshbP++UCEtqbwvveDRhmr5sMYkl+duEOca5156fcRy4tQ8Y3moNcKFKzHGMenShEIHz+W5KE=";
                var passCert = "12345678a";
                comprobante.Fecha = DateTime.Now.ToString("s");
                comprobante.Impuestos.Retenciones = null;
                foreach (var item in comprobante.Conceptos)
                {
                    item.Impuestos.Retenciones = null;
                }
                var cfdi = operacionesEmision.GeneraCfdi33(comprobante, noCertificado, certificado, key, passCert);

             

                //var certificado = @"Z:\Alexander\Mega\Personal\Trabajo\Proyectos\Mvc\Doc\Cert_Sellos\CSDAAA010101AAA\CSD01_AAA010101AAA.cer";
                //var bCer = File.ReadAllBytes(certificado);
                //var cerBase64 = Convert.ToBase64String(bCer);

                
            }
            catch (TimeoutException et)
            {
                Console.WriteLine(et.Message);
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
        }

    }
}
