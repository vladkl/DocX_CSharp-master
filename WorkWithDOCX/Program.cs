﻿using System.Diagnostics;
using System.Drawing;
using Xceed.Words.NET;
using Xceed.Document.NET;
using Font = Xceed.Document.NET.Font;
using System.IO;

namespace WorkWithDOCX
{
    class Program
    {

        private static int[] DiffCharCodes(string aText, bool ignoreCase)
        {
            int[] Codes;

            if (ignoreCase)
                aText = aText.ToUpperInvariant();

            Codes = new int[aText.Length];

            for (int n = 0; n < aText.Length; n++)
                Codes[n] = (int)aText[n];

            return (Codes);
        } // DiffCharCodes

        public static void Paragraphs(DocX doc,Paragraph p,string text,Highlight c)
        {
            string outfile = "XceedExample.docx";
            using (var document = DocX.Create(outfile))
            {
               // document.InsertParagraph("Formatted paragraphs").FontSize(15d).SpacingAfter(30);

                p.Append(text).Highlight(c)
                   // .Color(c)
                    .Font(new Font("Times New Roman"))
                    //.FontSize(25)
                    //.Bold()
                    //.Append(" containing a blue italic text.").Color(System.Drawing.Color.Blue)
                    //.Font(new Font("Arial"))
                    //.Italic()
                    //.SpacingAfter(40)
                    ;


            }
        }
        //public static void ExtractPackageObjects(string filePath)
        //{
        //    using (StreamReader sr = new StreamReader(filePath))
        //    {
        //        RtfReader reader = new RtfReader(sr);
        //        IEnumerator<RtfObject> enumerator = reader.Read().GetEnumerator();
        //        while (enumerator.MoveNext())
        //        {
        //            if (enumerator.Current.Text == "object")
        //            {
        //                if (RtfReader.MoveToNextControlWord(enumerator, "objclass"))
        //                {
        //                    string className = RtfReader.GetNextText(enumerator);
        //                    if (className == "Package")
        //                    {
        //                        if (RtfReader.MoveToNextControlWord(enumerator, "objdata"))
        //                        {
        //                            byte[] data = RtfReader.GetNextTextAsByteArray(enumerator);
        //                            using (MemoryStream packageData = new MemoryStream())
        //                            {
        //                                RtfReader.ExtractObjectData(new MemoryStream(data), packageData);
        //                                packageData.Position = 0;
        //                                PackagedObject po = PackagedObject.Extract(packageData);
        //                                File.WriteAllBytes(po.DisplayName, po.Data);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
        static void Main(string[] args)
        {
            string outfile = "XceedExample.docx";
            using (var document = DocX.Create(outfile))
            {
                var p = document.InsertParagraph();
                var s1 = "Отчет должен строится на основе данных, получаемых из следующих систем и подсистем/программных модулей:\n1.ИСУП = MS Project\n2.IBM Лотус, используемые подсистемы:\na.ПМ Поручения\nb.ПМ Согласование ПГ\nc.ПМ Согласования ТЗ\nd.ПМ Договоры";
                var s2 = "Отчет должен строится на основе данных, получаемых из следующих систем и подсистем/программных модулей:\n3.ИСУП = MS Project\n4IBM Лотус, используемые подсистемы:\na.ПМ Поручения\nb.ПМ Согласование ПГ";
                Paragraphs(document,p,s1, Highlight.none);
                p = document.InsertParagraph();
                p = document.InsertParagraph();
                Paragraphs(document, p, s2, Highlight.none);
                p = document.InsertParagraph();
                p = document.InsertParagraph();
                bool ignoreCase = false;
                int[] a_codes = null;
                int[] b_codes = null;
                a_codes = DiffCharCodes(s1,ignoreCase);
                b_codes = DiffCharCodes(s2, ignoreCase);

                Diff.Item[] diffs = Diff.DiffInt(a_codes, b_codes);
                p = document.InsertParagraph();
                string outtext = "";
                int pos = 0;
                for (int n = 0; n < diffs.Length; n++)
                {
                    Diff.Item it = diffs[n];
                    outtext = "";
                    // write unchanged chars
                    while ((pos < it.StartB) && (pos < b_codes.Length))
                    {
                        outtext += (char)(b_codes[pos]);
                        pos++;
                    } // while

                    Paragraphs(document, p, outtext, Highlight.none);

                    outtext = "";
                    // write deleted chars
                    if (it.deletedA > 0)
                    {
                        
                        for (int m = 0; m < it.deletedA; m++)
                        {
                            outtext += (char)(a_codes[it.StartA + m]);
                        } // for
                        Paragraphs(document, p, outtext, Highlight.red);
                    }

                    // write inserted chars
                    outtext = "";
                    if (pos < it.StartB + it.insertedB)
                    {

                        while (pos < it.StartB + it.insertedB)
                        {
                            outtext += (char)(b_codes[pos]);
                            pos++;
                        } // while
                        Paragraphs(document, p, outtext, Highlight.green);
                    } // if
                } // while
                outtext = "";
                // write rest of unchanged chars
                while (pos < b_codes.Length)
                {
                    outtext += (char)(b_codes[pos]);
                    pos++;
                } // while
                Paragraphs(document, p, outtext, Highlight.none);
                p = document.InsertParagraph();
                //p.Append("\u0001\u0005\0\0\u0002\0\0\0\u000e\0\0\0Excel.Sheet.8\0\0\0\0\0\0\0\0\0\0&\0\0РП\u0011аЎ±\u001aб\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0>\0\u0003\0юя\t\0\u0006\0\0\0\0\0\0\0\0\0\0\0\u0001\0\0\0\u0001\0\0\0\0\0\0\0\0\u0010\0\0\u0002\0\0\0\u0001\0\0\0юяяя\0\0\0\0\0\0\0\0яяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяэяяя\t\0\0\0юяяя\u0004\0\0\0\u0005\0\0\0\u0006\0\0\0\a\0\0\0\b\0\0\0\n\0\0\0\u0010\0\0\0\v\0\0\0\f\0\0\0\r\0\0\0\u000e\0\0\0\u000f\0\0\0\u0011\0\0\0юяяяюяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяR\0o\0o\0t\0 \0E\0n\0t\0r\0y\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\u0016\0\u0005\0яяяяяяяя\u0004\0\0\0 \b\u0002\0\0\0\0\0А\0\0\0\0\0\0F\0\0\0\0\0\0\0\0\0\0\0\0@к\u001dі+„Х\u0001\u0003\0\0\0\0\u0019\0\0\0\0\0\0\u0001\0O\0l\0e\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\n\0\u0002\u0001яяяяяяяяяяяя\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\u0014\0\0\0\0\0\0\0\u0003\0E\0P\0R\0I\0N\0T\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\u0010\0\u0002\0\u0001\0\0\0\u0003\0\0\0яяяя\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\u0001\0\0\0И\n\0\0\0\0\0\0\u0001\0C\0o\0m\0p\0O\0b\0j\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\u0012\0\u0002\u0001яяяяяяяяяяяя\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0-\0\0\0m\0\0\0\0\0\0\0юяяя\u0002\0\0\0\u0003\0\0\0\u0004\0\0\0\u0005\0\0\0\u0006\0\0\0\a\0\0\0\b\0\0\0\t\0\0\0\n\0\0\0\v\0\0\0\f\0\0\0\r\0\0\0\u000e\0\0\0\u000f\0\0\0\u0010\0\0\0\u0011\0\0\0\u0012\0\0\0\u0013\0\0\0\u0014\0\0\0\u0015\0\0\0\u0016\0\0\0\u0017\0\0\0\u0018\0\0\0\u0019\0\0\0\u001a\0\0\0\u001b\0\0\0\u001c\0\0\0\u001d\0\0\0\u001e\0\0\0\u001f\0\0\0 \0\0\0!\0\0\0\"\0\0\0#\0\0\0$\0\0\0%\0\0\0&\0\0\0'\0\0\0(\0\0\0)\0\0\0*\0\0\0+\0\0\0,\0\0\0юяяя.\0\0\0юяяяюяяя1\0\0\02\0\0\03\0\0\04\0\0\05\0\0\06\0\0\07\0\0\08\0\0\09\0\0\0:\0\0\0;\0\0\0<\0\0\0=\0\0\0>\0\0\0?\0\0\0@\0\0\0A\0\0\0B\0\0\0C\0\0\0D\0\0\0E\0\0\0F\0\0\0G\0\0\0H\0\0\0I\0\0\0J\0\0\0K\0\0\0L\0\0\0M\0\0\0N\0\0\0O\0\0\0P\0\0\0Q\0\0\0R\0\0\0S\0\0\0T\0\0\0U\0\0\0V\0\0\0W\0\0\0X\0\0\0Y\0\0\0Z\0\0\0юяяяюяяя]\0\0\0^\0\0\0_\0\0\0юяяяa\0\0\0b\0\0\0c\0\0\0юяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяяя\u0001\0\0\u0002\b\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\u0001\0\0\0l\0\0\0Ј\0\0\0\u0003\0\0\0Ч\u0002\0\0®\0\0\0\0\0\0\0\0\0\0\0g$\0\0З\b\0\0 EMF\0\0\u0001\0И\n\0\07\0\0\0\u0002\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0v\u0006\0\0#\t\0\0Т\0\0\0)\u0001\0\0\0\0\0\0\0\0\0\0\0\0\0\0Љ4\u0003\0]€\u0004\0F\0\0\0,\0\0\0 \0\0\0EMF+\u0001@\u0001\0\u001c\0\0\0\u0010\0\0\0\u0002\u0010АЫ\0\0\0\0И\0\0\0И\0\0\0F\0\0\0\\\0\0\0P\0\0\0EMF+\"@\u0004\0\f\0\0\0\0\0\0\0\u001e@\t\0\f\0\0\0\0\0\0\0$@\u0001\0\f\0\0\0\0\0\0\00@\u0002\0\u0010\0\0\0\u0004\0\0\0\0\0Ђ?!@\a\0\f\0\0\0\0\0\0\0\u0004@\0\0\f\0\0\0\0\0\0\0\u0018\0\0\0\f\0\0\0\0\0\0\0\u0019\0\0\0\f\0\0\0яяя\0\u0014\0\0\0\f\0\0\0\r\0\0\0\u0012\0\0\0\f\0\0\0\u0002\0\0\0!\0\0\0\b\0\0\0\"\0\0\0\f\0\0\0яяяя!\0\0\0\b\0\0\0\"\0\0\0\f\0\0\0яяяя\n\0\0\0\u0010\0\0\0\0\0\0\0\0\0\0\0!\0\0\0\b\0\0\0\u0019\0\0\0\f\0\0\0яяя\0\u0018\0\0\0\f\0\0\0\0\0\0\0\"\0\0\0\f\0\0\0яяяя!\0\0\0\b\0\0\0\u0019\0\0\0\f\0\0\0яяя\0\u0018\0\0\0\f\0\0\0\0\0\0\0\u001e\0\0\0\u0018\0\0\0\0\0\0\0\0\0\0\0Э\u0002\0\0°\0\0\0\"\0\0\0\f\0\0\0яяяя!\0\0\0\b\0\0\0\u0019\0\0\0\f\0\0\0яяя\0\u0018\0\0\0\f\0\0\0\0\0\0\0\u001e\0\0\0\u0018\0\0\0\0\0\0\0\0\0\0\0Э\u0002\0\0°\0\0\0R\0\0\0p\u0001\0\0\u0001\0\0\0дяяя\0\0\0\0\0\0\0\0\0\0\0\0ђ\u0001\0\0\0\0\0\0\0\0\0\0A\0r\0i\0a\0l\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\u0013\0\n\0\0\0И`e\u0001°з\u0013\0*µ\00Тй\u0013\0°з\u0013\0К`e\u0001\u001f\0\0\0&\0Љ\u0001\u001a\0\0\0\u0005µ\00Тй\u0013\0°з\u0013\0И`e\u0001 \0\0\0Lй\u0013\0\u001bь\00Тй\u0013\0\u0005\0\0\0И`e\u0001 \0\0\0^х\00Рй\u0013\0И`e\u0001 \0\0\0\0\0\0\0фл\u0013\0zц\00\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0A\0r\0i\0a\0l\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\u0001\0\0\0\0\0\0\0¬Б\u00010\0\0\0\0\0\0\0\0|и\u0013\0\0\0\0\0Ф\f\nХ\0\0\0\0&\0Љ\u0001Ф\f\nХЊ\f!ЕИ`e\u0001\u0005µ\00ћк\u0013\0dv\0\b\0\0\0\0%\0\0\0\f\0\0\0\u0001\0\0\0\u0014\0\0\0\f\0\0\0\r\0\0\0\u0012\0\0\0\f\0\0\0\u0001\0\0\0T\0\0\0T\0\0\0Ј\0\0\0\u0003\0\0\0І\0\0\0\"\0\0\0\u0001\0\0\0Ц$KAл)KAЈ\0\0\0\u0003\0\0\0\u0001\0\0\0L\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0яяяяяяяяP\0\0\01\0 \u0004\u0010\0\0\0T\0\0\0T\0\0\0Z\u0001\0\0\u0003\0\0\0i\u0001\0\0\"\0\0\0\u0001\0\0\0Ц$KAл)KAZ\u0001\0\0\u0003\0\0\0\u0001\0\0\0L\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0яяяяяяяяP\0\0\02\0\u0002\0\u0010\0\0\0T\0\0\0T\0\0\0\u0011\u0002\0\0\u0003\0\0\0 \u0002\0\0\"\0\0\0\u0001\0\0\0Ц$KAл)KA\u0011\u0002\0\0\u0003\0\0\0\u0001\0\0\0L\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0яяяяяяяяP\0\0\03\0\u0002\0\u0010\0\0\0T\0\0\0T\0\0\0И\u0002\0\0\u0003\0\0\0Ч\u0002\0\0\"\0\0\0\u0001\0\0\0Ц$KAл)KAИ\u0002\0\0\u0003\0\0\0\u0001\0\0\0L\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0яяяяяяяяP\0\0\04\0\0\0\u0010\0\0\0T\0\0\0T\0\0\0Ј\0\0\0&\0\0\0І\0\0\0E\0\0\0\u0001\0\0\0Ц$KAл)KAЈ\0\0\0&\0\0\0\u0001\0\0\0L\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0яяяяяяяяP\0\0\02\0\0\0\u0010\0\0\0T\0\0\0T\0\0\0Z\u0001\0\0&\0\0\0i\u0001\0\0E\0\0\0\u0001\0\0\0Ц$KAл)KAZ\u0001\0\0&\0\0\0\u0001\0\0\0L\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0яяяяяяяяP\0\0\04\0\u0003\0\u0010\0\0\0T\0\0\0T\0\0\0\u0011\u0002\0\0&\0\0\0 \u0002\0\0E\0\0\0\u0001\0\0\0Ц$KAл)KA\u0011\u0002\0\0&\0\0\0\u0001\0\0\0L\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0яяяяяяяяP\0\0\06\0\u0003\0\u0010\0\0\0T\0\0\0T\0\0\0И\u0002\0\0&\0\0\0Ч\u0002\0\0E\0\0\0\u0001\0\0\0Ц$KAл)KAИ\u0002\0\0&\0\0\0\u0001\0\0\0L\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0яяяяяяяяP\0\0\08\0\0\0\u0010\0\0\0T\0\0\0T\0\0\0Ј\0\0\0I\0\0\0І\0\0\0h\0\0\0\u0001\0\0\0Ц$KAл)KAЈ\0\0\0I\0\0\0\u0001\0\0\0L\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0яяяяяяяяP\0\0\03\0 \u0004\u0010\0\0\0T\0\0\0T\0\0\0Z\u0001\0\0I\0\0\0i\u0001\0\0h\0\0\0\u0001\0\0\0Ц$KAл)KAZ\u0001\0\0I\0\0\0\u0001\0\0\0L\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0яяяяяяяяP\0\0\06\0\u0003\0\u0010\0\0\0T\0\0\0T\0\0\0\u0011\u0002\0\0I\0\0\0 \u0002\0\0h\0\0\0\u0001\0\0\0Ц$KAл)KA\u0011\u0002\0\0I\0\0\0\u0001\0\0\0L\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0яяяяяяяяP\0\0\09\0\a\0\u0010\0\0\0T\0\0\0X\0\0\0ё\u0002\0\0I\0\0\0Ч\u0002\0\0h\0\0\0\u0001\0\0\0Ц$KAл)KAё\u0002\0\0I\0\0\0\u0002\0\0\0L\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0яяяяяяяяP\0\0\01\02\0\u0010\0\0\0\u0010\0\0\0T\0\0\0T\0\0\0Ј\0\0\0l\0\0\0І\0\0\0‹\0\0\0\u0001\0\0\0Ц$KAл)KAЈ\0\0\0l\0\0\0\u0001\0\0\0L\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0яяяяяяяяP\0\0\04\0\u0004\0\u0010\0\0\0T\0\0\0T\0\0\0Z\u0001\0\0l\0\0\0i\u0001\0\0‹\0\0\0\u0001\0\0\0Ц$KAл)KAZ\u0001\0\0l\0\0\0\u0001\0\0\0L\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0яяяяяяяяP\0\0\08\0\t\0\u0010\0\0\0T\0\0\0X\0\0\0\u0001\u0002\0\0l\0\0\0 \u0002\0\0‹\0\0\0\u0001\0\0\0Ц$KAл)KA\u0001\u0002\0\0l\0\0\0\u0002\0\0\0L\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0яяяяяяяяP\0\0\01\02\0\u0010\0\0\0\u0010\0\0\0T\0\0\0X\0\0\0ё\u0002\0\0l\0\0\0Ч\u0002\0\0‹\0\0\0\u0001\0\0\0Ц$KAл)KAё\u0002\0\0l\0\0\0\u0002\0\0\0L\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0яяяяяяяяP\0\0\01\06\0\u0010\0\0\0\u0010\0\0\0T\0\0\0T\0\0\0Ј\0\0\0Џ\0\0\0І\0\0\0®\0\0\0\u0001\0\0\0Ц$KAл)KAЈ\0\0\0Џ\0\0\0\u0001\0\0\0L\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0яяяяяяяяP\0\0\05\0\0\0\u0010\0\0\0T\0\0\0X\0\0\0J\u0001\0\0Џ\0\0\0i\u0001\0\0®\0\0\0\u0001\0\0\0Ц$KAл)KAJ\u0001\0\0Џ\0\0\0\u0002\0\0\0L\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0яяяяяяяяP\0\0\01\00\0\u0010\0\0\0\u0010\0\0\0T\0\0\0X\0\0\0\u0001\u0002\0\0Џ\0\0\0 \u0002\0\0®\0\0\0\u0001\0\0\0Ц$KAл)KA\u0001\u0002\0\0Џ\0\0\0\u0002\0\0\0L\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0яяяяяяяяP\0\0\01\05\0\u0010\0\0\0\u0010\0\0\0T\0\0\0X\0\0\0ё\u0002\0\0Џ\0\0\0Ч\u0002\0\0®\0\0\0\u0001\0\0\0Ц$KAл)KAё\u0002\0\0Џ\0\0\0\u0002\0\0\0L\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0яяяяяяяяP\0\0\02\00\0\u0010\0\0\0\u0010\0\0\0%\0\0\0\f\0\0\0\r\0\0Ђ(\0\0\0\f\0\0\0\u0001\0\0\0\"\0\0\0\f\0\0\0яяяяF\0\0\04\0\0\0(\0\0\0EMF+*@\0\0$\0\0\0\u0018\0\0\0\0\0Ђ?\0\0\0Ђ\0\0\0Ђ\0\0Ђ?\0\0\0Ђ\0\0\0ЂF\0\0\0\u001c\0\0\0\u0010\0\0\0EMF+\u0002@\0\0\f\0\0\0\0\0\0\0\u000e\0\0\0\u0014\0\0\0\0\0\0\0\u0010\0\0\0\u0014\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\u0001\0юя\u0003\n\0\0яяяя \b\u0002\0\0\0\0\0А\0\0\0\0\0\0F!\0\0\0Microsoft Office Excel Worksheet\0\u0006\0\0\0Biff8\0\u000e\0\0\0Excel.Sheet.8\0ф9Іq\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\u0003\0\r\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\u0003\0O\0b\0j\0I\0n\0f\0o\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\u0012\0\u0002\u0001\u0002\0\0\0\u0006\0\0\0яяяя\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0/\0\0\0\u0006\0\0\0\0\0\0\0W\0o\0r\0k\0b\0o\0o\0k\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\u0012\0\u0002\u0001яяяяяяяяяяяя\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\00\0\0\0‡\n\0\0\0\0\0\0\u0002\0O\0l\0e\0P\0r\0e\0s\00\00\00\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\u0018\0\u0002\0\u0005\0\0\0\a\0\0\0яяяя\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0[\0\0\0$\0\0\0\0\0\0\0\u0005\0S\0u\0m\0m\0a\0r\0y\0I\0n\0f\0o\0r\0m\0a\0t\0i\0o\0n\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0(\0\u0002\u0001яяяя\b\0\0\0яяяя\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\\\0\0\0Р\0\0\0\0\0\0\0\t\b\u0010\0\0\u0006\u0005\0s Н\aБА\0\0\u0006\u0003\0\0б\0\u0002\0°\u0004Б\0\u0002\0\0\0в\0\0\0\\\0p\0\a\0\0Ninguno                                                                                                      B\0\u0002\0°\u0004a\u0001\u0002\0\0\0А\u0001\0\0=\u0001\u0002\0\u0001\0њ\0\u0002\0\u000e\0Ю\0\b\0д^\0\0\u0004\0\0\u0003\u0019\0\u0002\0\0\0\u0012\0\u0002\0\0\0\u0013\0\u0002\0\0\0Ї\u0001\u0002\0\0\0ј\u0001\u0002\0\0\0=\0\u0012\0-\0-\0T\u0015\b\a<\0\0\0\0\0\u0001\0X\u0002@\0\u0002\0\0\0Ќ\0\u0002\0\0\0\"\0\u0002\0\0\0\u000e\0\u0002\0\u0001\0·\u0001\u0002\0\0\0Ъ\0\u0002\0\0\01\0\u001a\0И\0\0\0я\u007fђ\u0001\0\0\0\0\0\u0015\u0005\u0001A\0r\0i\0a\0l\01\0\u001a\0И\0\0\0я\u007fђ\u0001\0\0\0\0\0\u0015\u0005\u0001A\0r\0i\0a\0l\01\0\u001a\0И\0\0\0я\u007fђ\u0001\0\0\0\0\0\u0015\u0005\u0001A\0r\0i\0a\0l\01\0\u001a\0И\0\0\0я\u007fђ\u0001\0\0\0\0\0\u0015\u0005\u0001A\0r\0i\0a\0l\0\u001e\u00043\0\u0005\0\u0017\0\u0001#\0,\0#\0#\00\0\\\0 \0\"\0¬ \"\0;\0\\\0-\0#\0,\0#\0#\00\0\\\0 \0\"\0¬ \"\0\u001e\u0004=\0\u0006\0\u001c\0\u0001#\0,\0#\0#\00\0\\\0 \0\"\0¬ \"\0;\0[\0R\0e\0d\0]\0\\\0-\0#\0,\0#\0#\00\0\\\0 \0\"\0¬ \"\0\u001e\u0004?\0\a\0\u001d\0\u0001#\0,\0#\0#\00\0.\00\00\0\\\0 \0\"\0¬ \"\0;\0\\\0-\0#\0,\0#\0#\00\0.\00\00\0\\\0 \0\"\0¬ \"\0\u001e\u0004I\0\b\0\"\0\u0001#\0,\0#\0#\00\0.\00\00\0\\\0 \0\"\0¬ \"\0;\0[\0R\0e\0d\0]\0\\\0-\0#\0,\0#\0#\00\0.\00\00\0\\\0 \0\"\0¬ \"\0\u001e\u0004q\0*\06\0\u0001_\0-\0*\0 \0#\0,\0#\0#\00\0\\\0 \0\"\0¬ \"\0_\0-\0;\0\\\0-\0*\0 \0#\0,\0#\0#\00\0\\\0 \0\"\0¬ \"\0_\0-\0;\0_\0-\0*\0 \0\"\0-\0\"\0\\\0 \0\"\0¬ \"\0_\0-\0;\0_\0-\0@\0_\0-\0\u001e\u0004k\0)\03\0\u0001_\0-\0*\0 \0#\0,\0#\0#\00\0\\\0 \0_\0¬ _\0-\0;\0\\\0-\0*\0 \0#\0,\0#\0#\00\0\\\0 \0_\0¬ _\0-\0;\0_\0-\0*\0 \0\"\0-\0\"\0\\\0 \0_\0¬ _\0-\0;\0_\0-\0@\0_\0-\0\u001e\u0004Ѓ\0,\0>\0\u0001_\0-\0*\0 \0#\0,\0#\0#\00\0.\00\00\0\\\0 \0\"\0¬ \"\0_\0-\0;\0\\\0-\0*\0 \0#\0,\0#\0#\00\0.\00\00\0\\\0 \0\"\0¬ \"\0_\0-\0;\0_\0-\0*\0 \0\"\0-\0\"\0?\0?\0\\\0 \0\"\0¬ \"\0_\0-\0;\0_\0-\0@\0_\0-\0\u001e\u0004{\0+\0;\0\u0001_\0-\0*\0 \0#\0,\0#\0#\00\0.\00\00\0\\\0 \0_\0¬ _\0-\0;\0\\\0-\0*\0 \0#\0,\0#\0#\00\0.\00\00\0\\\0 \0_\0¬ _\0-\0;\0_\0-\0*\0 \0\"\0-\0\"\0?\0?\0\\\0 \0_\0¬ _\0-\0;\0_\0-\0@\0_\0-\0а\0\u0014\0\0\0\0\0хя \0\0\0\0\0\0\0\0\0\0\0А а\0\u0014\0\u0001\0\0\0хя \0\0ф\0\0\0\0\0\0\0\0А а\0\u0014\0\u0001\0\0\0хя \0\0ф\0\0\0\0\0\0\0\0А а\0\u0014\0\u0002\0\0\0хя \0\0ф\0\0\0\0\0\0\0\0А а\0\u0014\0\u0002\0\0\0хя \0\0ф\0\0\0\0\0\0\0\0А а\0\u0014\0\0\0\0\0хя \0\0ф\0\0\0\0\0\0\0\0А а\0\u0014\0\0\0\0\0хя \0\0ф\0\0\0\0\0\0\0\0А а\0\u0014\0\0\0\0\0хя \0\0ф\0\0\0\0\0\0\0\0А а\0\u0014\0\0\0\0\0хя \0\0ф\0\0\0\0\0\0\0\0А а\0\u0014\0\0\0\0\0хя \0\0ф\0\0\0\0\0\0\0\0А а\0\u0014\0\0\0\0\0хя \0\0ф\0\0\0\0\0\0\0\0А а\0\u0014\0\0\0\0\0хя \0\0ф\0\0\0\0\0\0\0\0А а\0\u0014\0\0\0\0\0хя \0\0ф\0\0\0\0\0\0\0\0А а\0\u0014\0\0\0\0\0хя \0\0ф\0\0\0\0\0\0\0\0А а\0\u0014\0\0\0\0\0хя \0\0ф\0\0\0\0\0\0\0\0А а\0\u0014\0\0\0\0\0\u0001\0 \0\0\0\0\0\0\0\0\0\0\0А а\0\u0014\0\u0001\0+\0хя \0\0ш\0\0\0\0\0\0\0\0А а\0\u0014\0\u0001\0)\0хя \0\0ш\0\0\0\0\0\0\0\0А а\0\u0014\0\u0001\0,\0хя \0\0ш\0\0\0\0\0\0\0\0А а\0\u0014\0\u0001\0*\0хя \0\0ш\0\0\0\0\0\0\0\0А а\0\u0014\0\u0001\0\t\0хя \0\0ш\0\0\0\0\0\0\0\0А “\u0002\u0004\0\u0010Ђ\u0003я“\u0002\u0004\0\u0011Ђ\u0006я“\u0002\u0004\0\u0012Ђ\u0004я“\u0002\u0004\0\u0013Ђ\aя“\u0002\u0004\0\0Ђ\0я“\u0002\u0004\0\u0014Ђ\u0005я`\u0001\u0002\0\0\0…\0\r\0\u0002\a\0\0\0\0\u0005\0Hoja1Њ\0\u0004\0\"\0\"\0Б\u0001\b\0Б\u0001\0\0\"ѕ\u0001\0ь\0\b\0\0\0\0\0\0\0\0\0я\0\u0002\0\b\0c\b\u0015\0c\b\0\0\0\0\0\0\0\0\0\0\u0015\0\0\0\0\0\0\0\u0002\n\0\0\0\t\b\u0010\0\0\u0006\u0010\0s Н\aБА\0\0\u0006\u0003\0\0\r\0\u0002\0\u0001\0\f\0\u0002\0d\0\u000f\0\u0002\0\u0001\0\u0011\0\u0002\0\0\0\u0010\0\b\0ь©сТMbP?_\0\u0002\0\u0001\0*\0\u0002\0\0\0+\0\u0002\0\0\0‚\0\u0002\0\u0001\0Ђ\0\b\0\0\0\0\0\0\0\0\0%\u0002\u0004\0\0\0я\0Ѓ\0\u0002\0Б\u0004\u0014\0\0\0\u0015\0\0\0ѓ\0\u0002\0\0\0„\0\u0002\0\0\0M\0n\u0001\0\0M\0i\0c\0r\0o\0s\0o\0f\0t\0 \0O\0f\0f\0i\0c\0e\0 \0D\0o\0c\0u\0m\0e\0n\0t\0 \0I\0m\0a\0g\0\0\0\0\0\u0001\u0004\0\u0004Ь\0ђ\0\u0003/\0\0\u0001\0\t\0\0\0\0\0d\0\u0001\0\u0001\0И\0\u0001\0\u0001\0И\0\u0001\0\0\0L\0e\0t\0t\0e\0r\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0widm\u0010\0\0\0\u0001\0\0\0\0\0\0\0\0\0\0\0ю\0\0\0\u0001\0\0\0\0\0\0\0И\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0Ў\0\"\0\t\0d\0\u0001\0\u0001\0\u0001\0\u0002\0И\0И\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\u0001\0U\0\u0002\0\n\0\0\u0002\u000e\0\0\0\0\0\u0005\0\0\0\0\0\u0004\0\0\0\b\u0002\u0010\0\0\0\0\0\u0004\0я\0\0\0\0\0\0\u0001\u000f\0\b\u0002\u0010\0\u0001\0\0\0\u0004\0я\0\0\0\0\0\0\u0001\u000f\0\b\u0002\u0010\0\u0002\0\0\0\u0004\0я\0\0\0\0\0\0\u0001\u000f\0\b\u0002\u0010\0\u0003\0\0\0\u0004\0я\0\0\0\0\0\0\u0001\u000f\0\b\u0002\u0010\0\u0004\0\0\0\u0004\0я\0\0\0\0\0\0\u0001\u000f\0Ѕ\0\u001e\0\0\0\0\0\u000f\0\0\0р?\u000f\0\0\0\0@\u000f\0\0\0\b@\u000f\0\0\0\u0010@\u0003\0Ѕ\0\u001e\0\u0001\0\0\0\u000f\0\0\0\0@\u000f\0\0\0\u0010@\u000f\0\0\0\u0018@\u000f\0\0\0 @\u0003\0Ѕ\0\u001e\0\u0002\0\0\0\u000f\0\0\0\b@\u000f\0\0\0\u0018@\u000f\0\0\0\"@\u000f\0\0\0(@\u0003\0Ѕ\0\u001e\0\u0003\0\0\0\u000f\0\0\0\u0010@\u000f\0\0\0 @\u000f\0\0\0(@\u000f\0\0\00@\u0003\0Ѕ\0\u001e\0\u0004\0\0\0\u000f\0\0\0\u0014@\u000f\0\0\0$@\u000f\0\0\0.@\u000f\0\0\04@\u0003\0Ч\0\u000e\0\u000e\u0001\0\0P\0\"\0\"\0\"\0\"\0>\u0002\u0012\0¶\u0006\0\0\0\0@\0\0\0\0\0\0\0\0\0\0\0\u001d\0\u000f\0\u0003\u0006\0\u0002\0\0\0\u0001\0\u0006\0\u0006\0\u0002\u0002п\0\u0006\0\0\07\0\0\0\n\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\u0004\0\0\0\u0001\0\0\0яяяя\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0юя\0\0\u0005\u0001\u0002\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\u0001\0\0\0а…џтщOh\u0010«‘\b\0+'іЩ0\0\0\0 \0\0\0\a\0\0\0\u0001\0\0\0@\0\0\0\u0004\0\0\0H\0\0\0\b\0\0\0X\0\0\0\u0012\0\0\0h\0\0\0\f\0\0\0Ђ\0\0\0\r\0\0\0Њ\0\0\0\u0013\0\0\0\u0098\0\0\0\u0002\0\0\0д\u0004\0\0\u001e\0\0\0\b\0\0\0Ninguno\0\u001e\0\0\0\b\0\0\0Ninguno\0\u001e\0\0\0\u0010\0\0\0Microsoft Excel\0@\0\0\0\0ўљ\u0010Ф\u0003К\u0001@\0\0\0ЂTЃ\u001fФ\u0003К\u0001\u0003\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\u0005\0D\0o\0c\0u\0m\0e\0n\0t\0S\0u\0m\0m\0a\0r\0y\0I\0n\0f\0o\0r\0m\0a\0t\0i\0o\0n\0\0\0\0\0\0\0\0\0\0\08\0\u0002\0яяяяяяяяяяяя\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0`\0\0\0ь\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0яяяяяяяяяяяя\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0яяяяяяяяяяяя\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0яяяяяяяяяяяя\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0юя\0\0\u0005\u0001\u0002\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\u0001\0\0\0\u0002ХНХњ.\u001b\u0010“—\b\0+,щ®0\0\0\0М\0\0\0\t\0\0\0\u0001\0\0\0P\0\0\0\u000f\0\0\0X\0\0\0\u0017\0\0\0h\0\0\0\v\0\0\0p\0\0\0\u0010\0\0\0x\0\0\0\u0013\0\0\0Ђ\0\0\0\u0016\0\0\0€\0\0\0\r\0\0\0ђ\0\0\0\f\0\0\0ў\0\0\0\u0002\0\0\0д\u0004\0\0\u001e\0\0\0\b\0\0\0Ninguna\0\u0003\0\0\0\u000f'\v\0\v\0\0\0\0\0\0\0\v\0\0\0\0\0\0\0\v\0\0\0\0\0\0\0\v\0\0\0\0\0\0\0\u001e\u0010\0\0\u0001\0\0\0\u0006\0\0\0Hoja1\0\f\u0010\0\0\u0002\0\0\0\u001e\0\0\0\u0011\0\0\0Hojas de cбlculo\0\u0003\0\0\0\u0001\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\0\u0001\u0005\0\0\0\0\0\0");

                document.Save();
                Process.Start("WinWord.exe", outfile);
            }
                
        }
    }
}