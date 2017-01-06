//********************************************************************************************
//Author: Sergey Stoyan
//        sergey.stoyan@gmail.com
//        http://www.cliversoft.com
//********************************************************************************************
using System;
using System.Data.Linq;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text.RegularExpressions;

namespace Cliver.PdfFixer
{
    class Pdf
    {       
        //class Field
        //{
        //    public string Name;
        //    public int Page;
        //    public PdfDictionary Pd;
        //}

        static public string Fix(string input_pdf, string output_pdf)
        {
            PdfReader.unethicalreading = true;

            PdfReader pr;
            pr = new PdfReader(input_pdf);
            MemoryStream ms = new MemoryStream();
            PdfStamper ps = new PdfStamper(pr,ms);
            
            List<KeyValuePair<string, AcroFields.Item>> afs = new List<KeyValuePair<string, AcroFields.Item>>();
            foreach (KeyValuePair<string, AcroFields.Item> kvp in ps.AcroFields.Fields)
                afs.Add(kvp);
            int k = 1;
            foreach (KeyValuePair<string, AcroFields.Item> kvp in afs)
            {
                //bool heal = false;
                //List<Field> fields = new List<Field>();
                if (kvp.Value.Size <= 1)
                    continue;
                for (int i = 0; i < kvp.Value.Size; i++)
                {
                    var m = kvp.Value.GetMerged(i);
                    //var ap = pd.Get(PdfName.AP);
                    //var ap = pd.Get(PdfName.MK);
                    //var ap = pd.Get(PdfName.Q);
                    var v = kvp.Value.GetValue(i);
                    var p = v.Get(PdfName.PARENT);
                    if (p != null)//remove
                    {
                        //k++;
                        v.Put(PdfName.T, new PdfString("---" + k++));
                        v.Put(PdfName.PARENT, null);
                        //v.Put(PdfName.KIDS, null);
                        var vi = kvp.Value.GetWidget(i);
                        //vi.Put(PdfName.PARENT, null);
                        //m.Put(PdfName.T, new PdfString("---" + k++));
                        //m.Put(PdfName.PARENT, null);
                        //m.Put(PdfName.KIDS, null);
                        //heal = true;
                    }
                    //else
                    //    fields.Add(new Field { Name = kvp.Key, Page = kvp.Value.GetPage(i), Pd = m });
                }
                //ps.AcroFields.RegenerateField(kvp.Key);
                //if (heal && fields.Count > 0 && false)
                //{
                //    int j = 1;
                //    int field_type = ps.AcroFields.GetFieldType(kvp.Key);
                //    ps.AcroFields.RemoveField(kvp.Key);
                //    ps.AcroFields.Fields.Remove(kvp.Key);
                //    foreach (Field f in fields)
                //    {
                //        var pa = (PdfArray)f.Pd.Get(PdfName.RECT);
                //        Rectangle r = new Rectangle(
                //            ((PdfNumber)pa[0]).FloatValue,
                //            ((PdfNumber)pa[1]).FloatValue,
                //            ((PdfNumber)pa[2]).FloatValue,
                //            ((PdfNumber)pa[3]).FloatValue
                //            );
                //        PdfFormField pff = null;
                //        string name = f.Name + (j > 0 ? "" + j : "");
                //        j++;
                //        switch (field_type)
                //        {
                //            case AcroFields.FIELD_TYPE_CHECKBOX:
                //                {
                //                    RadioCheckField tf = new RadioCheckField(ps.Writer, r, name, "on");
                //                    pff = tf.CheckField;
                //                }
                //                break;
                //            case AcroFields.FIELD_TYPE_COMBO:
                //                break;
                //            case AcroFields.FIELD_TYPE_LIST:
                //                break;
                //            case AcroFields.FIELD_TYPE_NONE:
                //                break;
                //            case AcroFields.FIELD_TYPE_PUSHBUTTON:
                //                {
                //                    PushbuttonField tf = new PushbuttonField(ps.Writer, r, name);
                //                    pff = tf.Field;
                //                }
                //                break;
                //            case AcroFields.FIELD_TYPE_RADIOBUTTON:
                //                {
                //                    RadioCheckField tf = new RadioCheckField(ps.Writer, r, name, "");
                //                    pff = tf.CheckField;
                //                }
                //                break;
                //            case AcroFields.FIELD_TYPE_SIGNATURE:
                //                break;
                //            case AcroFields.FIELD_TYPE_TEXT:
                //                {
                //                    TextField tf = new TextField(ps.Writer, r, name);
                //                    pff = tf.GetTextField();
                //                }
                //                break;
                //            default:
                //                break;
                //        }
                //        if (pff != null)
                //            ps.AddAnnotation(pff, f.Page);
                //        if (j > 0)
                //        {
                //            bool m = ps.AcroFields.RenameField(name, kvp.Key);
                //        }
                //    }
                //}
            }
            ps.Close();
            pr.Close();

            pr = new PdfReader(new MemoryStream(ms.GetBuffer()));
            ps = new PdfStamper(pr, new FileStream(output_pdf, FileMode.Create, FileAccess.Write, FileShare.None));
            //var x = ps.AcroFields.GetFieldItem("---");

            for (int i = 1; i <= k; i++)
            {
                bool h = ps.AcroFields.RemoveField("---" + i);
                bool g = ps.AcroFields.Fields.Remove("---" + i);
            }
            //ps.AcroFields.("---");

            string fs = "";
            foreach (KeyValuePair<string, AcroFields.Item> kvp in ps.AcroFields.Fields)
            {
                fs += "\n{\"" + kvp.Key + "\", \"" + kvp.Value + "\"},";
            }


            //foreach (KeyValuePair<string, string>kvp in fields2value)
            //    set_field(ps.AcroFields, kvp.Key, kvp.Value);
            //ps.FormFlattening = true;

            //var pcb = ps.GetOverContent(1);
            //add_image(pcb, employee_signature, new System.Drawing.Point(140, 213));
            //add_image(pcb, preparer_signature, new System.Drawing.Point(180, 120));
            //pcb = ps.GetOverContent(2);
            //add_image(pcb, employer_signature, new System.Drawing.Point(60, 256));
            //add_image(pcb, employer_signature, new System.Drawing.Point(65, 30));
            ps.Close();
            pr.Close();
            
            //pr = new PdfReader(new MemoryStream(ms.GetBuffer()));
            //using (Stream output = new FileStream(output_pdf, FileMode.Create, FileAccess.Write, FileShare.None))
            //{
            //    PdfEncryptor.Encrypt(pr, output, true, user_password, owner_password, PdfWriter.ALLOW_SCREENREADERS);
            //}
            //pr.Close();
            return output_pdf;
        }

        static void set_field(AcroFields form, string field_key, string value)
        {
            switch (form.GetFieldType(field_key))
            {
                case AcroFields.FIELD_TYPE_CHECKBOX:
                case AcroFields.FIELD_TYPE_RADIOBUTTON:
                    //bool v;
                    //if (bool.TryParse(value, out v))
                    //    value = !v ? "false" : "true";
                    //else
                    //{
                    //    int i;
                    //    if (int.TryParse(value, out i))
                    //        value = i == 0 ? "false" : "true";
                    //    else
                    //        value = string.IsNullOrEmpty(value) ? "false" : "true";
                    //}
                    //form.SetField(field_key, value);
                    //break;
                case AcroFields.FIELD_TYPE_COMBO:
                case AcroFields.FIELD_TYPE_LIST:
                case AcroFields.FIELD_TYPE_NONE:
                case AcroFields.FIELD_TYPE_PUSHBUTTON:
                case AcroFields.FIELD_TYPE_SIGNATURE:
                case AcroFields.FIELD_TYPE_TEXT:
                    form.SetField(field_key, value);
                    break;
                default:
                    throw new Exception("Unknown option: " + form.GetFieldType(field_key));
            }
        }
    }
}