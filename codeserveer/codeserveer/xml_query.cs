using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace codeserveer
{
    public class xml_query
    {
        //объявление xml документа
        //public XDocument code_server_query;
        //Строка запроса
        public string test;
        //
        //Описание тела запроса
       

        //инициализация
        public xml_query()
        {
        }
     

        //болванка
        public string xml_create_template(string _name_txt, string _id_txt,
            string _number_txt, string _maxchannels_txt, string _datetime_txt, 
            List<string> _value_txt, string _status_txt)
        {
            
            XDocument code_server_query = new XDocument();
            XElement driver = new XElement("driver");
            XElement info = new XElement("info");
            XElement id = new XElement("id");
            XElement name = new XElement("name");
            XElement maxchannels = new XElement("maxchannels");
            XElement data = new XElement("data");
            XElement datetime = new XElement("datetime");
            XElement channel = new XElement("channel");
            XElement number = new XElement("number");
            XElement value = new XElement("value");
            XElement status = new XElement("status");

            //Дефотные значения
            XText id_txt = new XText(_id_txt);
            XText name_txt = new XText(_name_txt);
            XText maxchannels_txt = new XText(_maxchannels_txt);
            //XText datetime_txt = new XText("20160329114152200");
            XText number_txt = new XText(_number_txt);
            XText value_txt = new XText(_value_txt[0]);
            XText status_txt = new XText(_status_txt);
        

            

            id.Add(id_txt);
            name.Add(name_txt);
            maxchannels.Add(maxchannels_txt);
            //datetime.Add(datetime_txt);
            
            number.Add(number_txt);
            value.Add(value_txt);
            status.Add(status_txt);

            info.Add(id);
            info.Add(name);
            info.Add(maxchannels);

            //data.Add(datetime);
            channel.Add(number);
            channel.Add(value);
            channel.Add(status);

            for (int i = 0; i < Convert.ToInt32(maxchannels.Value); i++)
            {
                //несколько! каналов
                
                
                data.Add(channel);

                
                number.Value = Convert.ToString(Convert.ToInt32(_number_txt) + i);
                value.Value = _value_txt[i];
            }

            
            driver.Add(info);
            driver.Add(data);
            code_server_query.Add(driver);

           
            return test =code_server_query.ToString();
           

            //a = 20;

            //Например:
            //<driver>
            //     <info>
            //         <id>123</id>
            //         <name>Хроматограф СГА-05</name>
            //         <maxchannels>8</maxchannels>
            //     </info>
            //     <data>
            //         <datetime>20070205114152200</datetime>   - это время 5.2.2007 11:41:52.200
            //         <channel>
            //             <number>1</number>
            //             <value>1234567890</value>
            //             <status>1</status>
            //         </channel>
            //         <channel>
            //             <number>2</number>
            //             <value>12345</value>
            //             <status>1</status>
            //         </channel>
            //         <channel>
            //             <number>5</number>
            //             <value>67890</value>
            //             <status>1</status>
            //         </channel>
            //     </data>
            //</driver>
    //        XDocument xdoc = new XDocument(new XElement("phones",
    //new XElement("phone",
    //    new XAttribute("name", "iPhone 6"),
    //    new XElement("company", "Apple"),
    //    new XElement("price", "40000")),
    //new XElement("phone",
    //    new XAttribute("name", "Samsung Galaxy S5"),
    //    new XElement("company", "Samsung"),
    //    new XElement("price", "33000"))));
    //        xdoc.Save("phones.xml");
        }
        //Текущая дата
        public string get_time(){
            DateTime now = DateTime.Now;
            string string_time = now.Year.ToString()+now.Month.ToString()+
                now.Day.ToString()+now.Hour.ToString()+now.Minute.ToString()+
                now.Second.ToString()+now.Millisecond.ToString();
            return string_time;
        
        }

        // Смена значения 
        public void change_data_value(string val) {

            string val1 = val;
            //value.Value=val1;
            //try
            //{
            //    test = code_server_query.ToString();
            //}
            //catch { }
        }


    }
}
