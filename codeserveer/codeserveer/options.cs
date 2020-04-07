using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace codeserveer
{
    public class options
    {
        public System.Windows.Forms.TreeViewAero treeview_opt = new System.Windows.Forms.TreeViewAero();

        public options() { }
        //public string bilet;
        //public bilets = new List<typeof(bilet)>();
        public string cur_path = Directory.GetCurrentDirectory() + "//options_tree.xml";
        

       

        //Новый билет
        //public void new_bilet(){
        //    bilet cur_bilet = new bilet();
        //    //cur_bilet.numb_bil = 0;
        //    bilets.Add(cur_bilet);
        //}


        //Запись серилизуемого объекта
        //public void WriteXML(out System.Windows.Forms.TreeViewAero tree)
        //{

        //    System.Xml.Serialization.XmlSerializer writer =
        //        new System.Xml.Serialization.XmlSerializer(typeof(object));
        //    System.IO.FileStream file = System.IO.File.Create(cur_path);
        //    writer.Serialize(file, tree);
        //    file.Close();
        //}
        

        //Чтение серилизуемого объекта
        public void ReadXML( out System.Windows.Forms.TreeViewAero tree)
        {
            //List<object> trees = new List<object>();
            //trees.Add(tree);
            //treeview_opt = tree;
            System.Xml.Serialization.XmlSerializer reader =
                new System.Xml.Serialization.XmlSerializer(typeof(object));
            System.IO.StreamReader file = new System.IO.StreamReader(
                cur_path);

            tree = (System.Windows.Forms.TreeViewAero)reader.Deserialize(file);

            //Console.WriteLine(overview.vopros);

        }
    }
}
