using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppWeb.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace AppWeb.Controllers
{
    //Entity framework solo se utiliza en el controlador, aqui hacemos nuestra interacción con nuestro mapeo
    public class MarcaController : Controller
    {

        public FileResult GenerarExcel()
        {
            byte[] buffer;

            using (MemoryStream ms = new MemoryStream())
            {
                //Todo el documento Excel
                ExcelPackage ep = new ExcelPackage();
                //Crear hoja
                ep.Workbook.Worksheets.Add("Reporte");

                ExcelWorksheet ew = ep.Workbook.Worksheets[0];

                //Nombre de columnas
                ew.Cells[1, 1].Value = "Id Marca";
                ew.Cells[1, 2].Value = "Nombre";
                ew.Cells[1, 3].Value = "Descripción";

                ew.Column(1).Width = 20;
                ew.Column(2).Width = 40;
                ew.Column(3).Width = 180;

                using (var Range = ew.Cells[1, 1, 1, 3])
                {
                    Range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    Range.Style.Font.Color.SetColor(Color.White);
                    Range.Style.Fill.BackgroundColor.SetColor(Color.DarkRed);
                }

                List<MarcaCls> Lista = (List<MarcaCls>)Session["Lista"];

                int nregistros = Lista.Count;
                //Pendiente
                for (int i = 0; i < nregistros; i++)
                {

                }
                //Pendiente
                ep.SaveAs(ms);
                buffer = ms.ToArray();
                return File(buffer, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
        }

        public FileResult GenerarPdf()
        {
            Document Doc = new Document();
            byte[] Buffer;
            using (MemoryStream Ms = new MemoryStream())
            {
                PdfWriter.GetInstance(Doc, Ms);
                Doc.Open();

                Paragraph Title = new Paragraph("Lista Marca");
                Title.Alignment = Element.ALIGN_CENTER;
                Doc.Add(Title);

                Paragraph Espacio = new Paragraph("\n");
                Doc.Add(Espacio);

                //Número de columnas (3) de tabla
                PdfPTable Table = new PdfPTable(3);
                //Anchos columnas
                float[] Values = new float[3] { 30, 40, 80 };
                //Anchos a la tabla
                Table.SetWidths(Values);
                //Celdas y contenido - Color-alineado y centrado
                PdfPCell Celda1 = new PdfPCell(new Phrase("Id Marca"));
                Celda1.BackgroundColor = new BaseColor(130, 130, 130);
                Celda1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                Table.AddCell(Celda1);

                PdfPCell Celda2 = new PdfPCell(new Phrase("Nombre"));
                Celda2.BackgroundColor = new BaseColor(130, 130, 130);
                Celda2.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                Table.AddCell(Celda2);

                PdfPCell Celda3 = new PdfPCell(new Phrase("Descripción"));
                Celda3.BackgroundColor = new BaseColor(130, 130, 130);
                Celda3.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                Table.AddCell(Celda3);

                List<MarcaCls> Lista = (List<MarcaCls>)Session["Lista"];

                int nregistros = Lista.Count;
                for (int i = 0; i < nregistros; i++)
                {
                    Table.AddCell(Lista[i].IidMarca.ToString());
                    Table.AddCell(Lista[i].Nombre);
                    Table.AddCell(Lista[i].Descripcion);
                }

                Doc.Add(Table);
                Doc.Close();

                Buffer = Ms.ToArray();
            }

            return File(Buffer, "application/pdf");

        }

        //GET: Marca
        //Los métodos del controller son las acciones que se utilizan en el RouteConfig
        public ActionResult Index(MarcaCls objMarcaCls)
        {
            //Para retornar la vista se debe declarar la lista fuera del using
            string NombreMarca = objMarcaCls.Nombre;
            List<MarcaCls> ListaMarca = null;
            //abre la conexion al abrir y cerrar el using y al instanciar a mi mapeo
            using (var bd = new BDPasajeEntities())
            {
                if (objMarcaCls.Nombre == null)
                {
                    //Marca recorre cada fila de nuestra tabla
                    ListaMarca = (from Obj in bd.Marca
                                  where Obj.BHABILITADO == 1
                                  select new MarcaCls
                                  {
                                      IidMarca = Obj.IIDMARCA,
                                      Nombre = Obj.NOMBRE,
                                      Descripcion = Obj.DESCRIPCION
                                  }).ToList();
                    Session["Lista"] = ListaMarca;
                }
                else
                {
                    ListaMarca = (from Obj in bd.Marca
                                  where Obj.BHABILITADO == 1
                                  //el método contains funciona como un %like%
                                  && Obj.NOMBRE.Contains(NombreMarca)
                                  select new MarcaCls
                                  {
                                      IidMarca = Obj.IIDMARCA,
                                      Nombre = Obj.NOMBRE,
                                      Descripcion = Obj.DESCRIPCION
                                  }).ToList();
                    Session["Lista"] = ListaMarca;
                }
            }
            return View(ListaMarca);
        }

        //Solo retorna la vista para el menú
        public ActionResult Agregar()
        {
            return View();
        }


        //Este realiza la inserción
        [HttpPost]
        public ActionResult Agregar(MarcaCls objMarcaCls)
        {
            int NumRegEncontrados = 0;
            string NombreMarca = objMarcaCls.Nombre;

            using (var Bd = new BDPasajeEntities())
            {
                NumRegEncontrados = Bd.Marca.Where(p => p.NOMBRE.Equals(NombreMarca)).Count();
            }

            if (!ModelState.IsValid || NumRegEncontrados >= 1)
            {
                if (NumRegEncontrados >= 1) objMarcaCls.MensajeError = "El nombre de la marca ya existe";
                return View(objMarcaCls);
            }
            else
            {
                using (var Bd = new BDPasajeEntities())
                {

                    Marca objMarca = new Marca();
                    objMarca.NOMBRE = objMarcaCls.Nombre;
                    objMarca.DESCRIPCION = objMarcaCls.Descripcion;
                    objMarca.BHABILITADO = 1;
                    Bd.Marca.Add(objMarca);
                    Bd.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        //Vista para cargar la ventana con los datos extraidos de la base de datos mediante Id
        public ActionResult Editar(int Id)
        {
            MarcaCls objMarcaCls = new MarcaCls();

            using (var Bd = new BDPasajeEntities())
            {
                Marca objMarca = Bd.Marca.Where(p => p.IIDMARCA.Equals(Id)).First();
                objMarcaCls.IidMarca = objMarca.IIDMARCA;
                objMarcaCls.Nombre = objMarca.NOMBRE;
                objMarcaCls.Descripcion = objMarca.DESCRIPCION;
            }

            return View(objMarcaCls);
        }

        [HttpPost]
        public ActionResult Editar(MarcaCls objMarcaCls)
        {
            int NumRegEncontrados = 0;
            string NombreMarca = objMarcaCls.Nombre;
            int IdMarca1 = objMarcaCls.IidMarca;

            using (var Bd = new BDPasajeEntities())
            {
                NumRegEncontrados = Bd.Marca.Where(p => p.NOMBRE.Equals(NombreMarca) && !p.IIDMARCA.Equals(IdMarca1)).Count();
            }

            if (!ModelState.IsValid || NumRegEncontrados >= 1)
            {
                if (NumRegEncontrados >= 1) objMarcaCls.MensajeError = "Esa marca ya se encunetra registrada";
                return View(objMarcaCls);
            }

            int IdMarca = objMarcaCls.IidMarca;
            using (var Bd = new BDPasajeEntities())
            {
                Marca objMarca = Bd.Marca.Where(p => p.IIDMARCA.Equals(IdMarca)).First();

                objMarca.NOMBRE = objMarcaCls.Nombre;
                objMarca.DESCRIPCION = objMarcaCls.Descripcion;
                Bd.SaveChanges();

            }

            return RedirectToAction("Index");
        }

        public ActionResult Eliminar(int Id)
        {

            using (var Bd = new BDPasajeEntities())
            {

                Marca objMarca = Bd.Marca.Where(p => p.IIDMARCA.Equals(Id)).First();
                objMarca.BHABILITADO = 0;
                Bd.SaveChanges();

            }

            return RedirectToAction("Index");
        }


    }
}