using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AdysTech.InfluxDB.Client.Net;
using FluentModbus;

namespace SENTRON_PAC5200_2InfluxDB
{
    class Program
    {
        static readonly ModbusTcpClient client = new ModbusTcpClient();
        static async Task Main(string[] args)
        {
            string SENTRON_IP = Environment.GetEnvironmentVariable("SENTRON_IP");
            string SENTRON_ID = Environment.GetEnvironmentVariable("SENTRON_ID");
            string INFLUXDB_IP = Environment.GetEnvironmentVariable("INFLUXDB_IP");
            string INFLUXDB_PORT = Environment.GetEnvironmentVariable("INFLUXDB_PORT");
            string INFLUXDB_DATABASE = Environment.GetEnvironmentVariable("INFLUXDB_DATABASE");
            string INFLUXDB_USERNAME = Environment.GetEnvironmentVariable("INFLUXDB_USERNAME");
            string INFLUXDB_PASSWORD = Environment.GetEnvironmentVariable("INFLUXDB_PASSWORD");
            int RETRY, READING;
            if (!Int32.TryParse(Environment.GetEnvironmentVariable("RETRY"), out RETRY))
            {
                Console.WriteLine("Invalid RETRY using 10");
                RETRY = 10 * 1000;
            }
            else
            {
                RETRY = RETRY * 1000;
            }
            if (!Int32.TryParse(Environment.GetEnvironmentVariable("READING"), out READING))
            {
                Console.WriteLine("Invalid READING using 5");
                READING = 5 * 1000;
            }
            else
            {
                READING = READING * 1000;
            }

            string statusURLString = "http://" + SENTRON_IP + "/";
            string influxDbURL = "http://" + INFLUXDB_IP + ":" + INFLUXDB_PORT;

            Console.WriteLine("Running with below parameters");
            Console.WriteLine("\tSENTRON_IP {0}", SENTRON_IP);
            Console.WriteLine("\tSENTRON_ID {0}", SENTRON_ID);
            Console.WriteLine("\tINFLUXDB_IP {0}", INFLUXDB_IP);
            Console.WriteLine("\tINFLUXDB_PORT {0}", INFLUXDB_PORT);
            Console.WriteLine("\tINFLUXDB_DATABASE {0}", INFLUXDB_DATABASE);
            Console.WriteLine("\tINFLUXDB_USERNAME {0}", INFLUXDB_USERNAME);
            Console.WriteLine("\tINFLUXDB_PASSWORD {0}", INFLUXDB_PASSWORD);
            Console.WriteLine("\tRETRY {0}", RETRY / 1000);
            Console.WriteLine("\tREADING {0}", READING / 1000);
            Console.WriteLine("\n\nUsing Status URL   : {0}", statusURLString);
            Console.WriteLine("Using InfluxDB URL : {0}", influxDbURL);


            while (true)
            {
                InfluxDBClient influxClient = new InfluxDBClient(influxDbURL, INFLUXDB_USERNAME, INFLUXDB_PASSWORD);
                try
                {
                    while (true)
                    {
                        byte unitIdentifier = 0xFF;

                        client.Connect(IPAddress.Parse(SENTRON_IP));
                        var data = client.ReadHoldingRegisters(unitIdentifier, 200, 122).ToArray();

                        var valMixed = new InfluxDatapoint<InfluxValueField>();
                        valMixed.UtcTimestamp = DateTime.UtcNow;
                        valMixed.Tags.Add("id", SENTRON_ID);
                        valMixed.MeasurementName = "METER_DATA";
                        valMixed.Precision = TimePrecision.Seconds;

                        var Va = ToFloatStart201(data, 201);
                        var Vb = ToFloatStart201(data, 203);
                        var Vc = ToFloatStart201(data, 205);
                        var VN = ToFloatStart201(data, 207);

                        valMixed.Fields.Add("Va", new InfluxValueField(Va));
                        valMixed.Fields.Add("Vb", new InfluxValueField(Vb));
                        valMixed.Fields.Add("Vc", new InfluxValueField(Vc));
                        valMixed.Fields.Add("VN", new InfluxValueField(VN));

                        var Ia = ToFloatStart201(data, 209);
                        var Ib = ToFloatStart201(data, 211);
                        var Ic = ToFloatStart201(data, 213);
                        var IN = ToFloatStart201(data, 215);

                        valMixed.Fields.Add("Ia", new InfluxValueField(Ia));
                        valMixed.Fields.Add("Ib", new InfluxValueField(Ib));
                        valMixed.Fields.Add("Ic", new InfluxValueField(Ic));
                        valMixed.Fields.Add("IN", new InfluxValueField(IN));

                        var Vab = ToFloatStart201(data, 217);
                        var Vbc = ToFloatStart201(data, 219);
                        var Vca = ToFloatStart201(data, 221);

                        valMixed.Fields.Add("Vab", new InfluxValueField(Vab));
                        valMixed.Fields.Add("Vbc", new InfluxValueField(Vbc));
                        valMixed.Fields.Add("Vca", new InfluxValueField(Vca));

                        var Vavg = ToFloatStart201(data, 223);
                        var Iavg = ToFloatStart201(data, 225);

                        valMixed.Fields.Add("Vavg", new InfluxValueField(Vavg));
                        valMixed.Fields.Add("Iavg", new InfluxValueField(Iavg));

                        var Pa = ToFloatStart201(data, 227);
                        var Pb = ToFloatStart201(data, 229);
                        var Pc = ToFloatStart201(data, 231);
                        var P = ToFloatStart201(data, 233);

                        valMixed.Fields.Add("Pa", new InfluxValueField(Pa));
                        valMixed.Fields.Add("Pb", new InfluxValueField(Pb));
                        valMixed.Fields.Add("Pc", new InfluxValueField(Pc));
                        valMixed.Fields.Add("P", new InfluxValueField(P));

                        var Qa = ToFloatStart201(data, 235);
                        var Qb = ToFloatStart201(data, 237);
                        var Qc = ToFloatStart201(data, 239);
                        var Q = ToFloatStart201(data, 241);

                        var Sa = ToFloatStart201(data, 243);
                        var Sb = ToFloatStart201(data, 245);
                        var Sc = ToFloatStart201(data, 247);
                        var S = ToFloatStart201(data, 249);

                        var cos_phi_a = ToFloatStart201(data, 251);
                        var cos_phi_b = ToFloatStart201(data, 253);
                        var cos_phi_c = ToFloatStart201(data, 255);
                        var cos_phi = ToFloatStart201(data, 257);

                        var PFa = ToFloatStart201(data, 259);
                        var PFb = ToFloatStart201(data, 261);
                        var PFc = ToFloatStart201(data, 263);
                        var PF = ToFloatStart201(data, 265);
                        
                        valMixed.Fields.Add("PFa", new InfluxValueField(PFa));
                        valMixed.Fields.Add("PFb", new InfluxValueField(PFb));
                        valMixed.Fields.Add("PFc", new InfluxValueField(PFc));
                        valMixed.Fields.Add("PF", new InfluxValueField(PF));

                        var phi_a = ToFloatStart201(data, 267);
                        var phi_b = ToFloatStart201(data, 269);
                        var phi_c = ToFloatStart201(data, 271);
                        var phi = ToFloatStart201(data, 273);

                        var f = ToFloatStart201(data, 275);
                        var U2 = ToFloatStart201(data, 277);

                        var THDS_Va_Vab = ToFloatStart201(data, 295);
                        var THDS_Vb_Vbc = ToFloatStart201(data, 297);
                        var THDS_Vc_Vca = ToFloatStart201(data, 299);

                        var THDS_Ia = ToFloatStart201(data, 301);
                        var THDS_Ib = ToFloatStart201(data, 303);
                        var THDS_Ic = ToFloatStart201(data, 305);

                        var V_phi_12 = ToFloatStart201(data, 307);
                        var V_phi_13 = ToFloatStart201(data, 309);
                        var I_phi_12 = ToFloatStart201(data, 311);
                        var I_phi_13 = ToFloatStart201(data, 313);

                        var Q1a = ToFloatStart201(data, 315);
                        var Q1b = ToFloatStart201(data, 317);
                        var Q1c = ToFloatStart201(data, 319);
                        var Q1 = ToFloatStart201(data, 321);

                        var r = await influxClient.PostPointAsync(INFLUXDB_DATABASE, valMixed);
                        Console.WriteLine(r);
                        Thread.Sleep(READING);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
                client.Disconnect();
                influxClient.Dispose();
                Thread.Sleep(RETRY);
            }
        }

        static float ToFloatStart201(byte[] input, int index)
        {
            index = index - 201;
            index = index / 2;
            index = index * 4;
            byte[] newArray = new[] { input[3 + index], input[2 + index], input[1 + index], input[0 + index] };
            return BitConverter.ToSingle(newArray, 0);
        }
    }
}
