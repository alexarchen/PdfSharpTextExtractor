using System;
using System.Text;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Content;
using PdfSharp.Pdf.Content.Objects;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf.Advanced;
using System.Linq;
using PdfSharp.Drawing;
namespace PdfTextExtractor
{

    
    class Program
    {
        static void Main(string[] args)
        {
            
            using (var _document = PdfReader.Open("license.pdf", PdfDocumentOpenMode.ReadOnly))
            {
                StringBuilder result = new StringBuilder();
                Extractor ext = new Extractor();
                foreach (var page in _document.Pages)
                {
                    ext.page = page;
                    ext.ExtractText(ContentReader.ReadContent(page), result);
                    result.AppendLine();
                }
               //Console.Write(result.ToString());
            }

        }

public class Extractor{
        PdfFont font;
        public PdfPage page {get; set;}
    
       public void ExtractText(CObject obj, StringBuilder target)
        {
            if (obj is CArray)
                ExtractText((CArray)obj, target);
            else if (obj is CComment)
                ExtractText((CComment)obj, target);
            else if (obj is CInteger)
                ExtractText((CInteger)obj, target);
            else if (obj is CName)
                ExtractText((CName)obj, target);
            else if (obj is CNumber)
                ExtractText((CNumber)obj, target);
            else if (obj is COperator)
                ExtractText((COperator)obj, target);
            else if (obj is CReal)
                ExtractText((CReal)obj, target);
            else if (obj is CSequence)
                ExtractText((CSequence)obj, target);
            else if (obj is CString)
                ExtractText((CString)obj, target);
            else
                throw new NotImplementedException(obj.GetType().AssemblyQualifiedName);
        }

        private void ExtractText(CArray obj, StringBuilder target)
        {
            foreach (var element in obj)
            {
                ExtractText(element, target);
            }
        }
        private void ExtractText(CComment obj, StringBuilder target) { /* nothing */ }
        private void ExtractText(CInteger obj, StringBuilder target) { /* nothing */ }
        private void ExtractText(CName obj, StringBuilder target) { /* nothing */ }
        private void ExtractText(CNumber obj, StringBuilder target) { /* nothing */ }
        private void ExtractText(COperator obj, StringBuilder target)
        {
            Console.WriteLine("Op: "+obj.OpCode.OpCodeName+" N: "+obj.Operands.Count);


            if (obj.OpCode.OpCodeName == OpCodeName.QuoteSingle || obj.OpCode.OpCodeName == OpCodeName.QuoteDbl || obj.OpCode.OpCodeName == OpCodeName.Tj || obj.OpCode.OpCodeName == OpCodeName.TJ)
            {
                /*foreach (var element in obj.Operands)
                {
                    Console.WriteLine("   "+element.ToString());
                    ExtractText(element, target);
                }*/
                //target.Append(" ");
                if (obj.OpCode.OpCodeName == OpCodeName.QuoteSingle || obj.OpCode.OpCodeName ==  OpCodeName.QuoteDbl) target.Append("\n");

                if (obj.Operands.Count==1)
                {
                   foreach (var elem in ((CArray)(obj.Operands[0]))){

                       if (elem is CString){
                           target.Append(elem.ToString());
                       }
                       else{
                        if ((elem is CNumber) && (obj.OpCode.OpCodeName == OpCodeName.Tj))
                          if (GetNumberValue((CNumber)elem)>750) {
                              target.Append(" ");
                          }    
                       }
                   }
                }else 
                 Console.WriteLine("Error TJ!");
            }
            else
            if ((obj.OpCode.OpCodeName == OpCodeName.Tx) || (obj.OpCode.OpCodeName == OpCodeName.TD) || (obj.OpCode.OpCodeName == OpCodeName.Td)){
                target.Append("\n");
               
            }
            else
            if (obj.OpCode.OpCodeName == OpCodeName.Tm)
            { 
                target.Append(" ");
            }
            else
            if (obj.OpCode.OpCodeName == OpCodeName.Tf)
            {
               if (obj.Operands.Count==2){
                   //if (obj.Operands[0] is CString)
                    {
                     string nF=obj.Operands[0].ToString();
                     XGraphics gfx = XGraphics.FromPdfPage(page);
                     
                     object fobj = (page.Resources.Elements["/Font"] as PdfDictionary).Elements[nF];
                     if (fobj is PdfReference) 
                     {
                         fobj = ((PdfReference)fobj).Value;
                     }
                     font = fobj as PdfFont;

                     //font = page.Resources.Elements["/Font"];
                    }
               }
               else
               {
                   Console.WriteLine("Error in Tf operator");
               }
            }   
            
        }

        double GetNumberValue(CNumber numb){
            if (numb is CReal) return ((CReal)numb).Value;
            else
            if (numb is CInteger) return ((CInteger)numb).Value;
            else return double.NaN;
        }



        private void ExtractText(CReal obj, StringBuilder target) { /* nothing */ }
        private void ExtractText(CSequence obj, StringBuilder target)
        {
            foreach (var element in obj)
            {
                ExtractText(element, target);
            }
        }
        private void ExtractText(CString obj, StringBuilder target)
        {
            target.Append(obj.Value);
        }

      }
    }
}
