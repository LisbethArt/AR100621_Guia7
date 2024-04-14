using Microsoft.AspNetCore.Mvc;
using SitioWeb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SitioWeb.Controllers
{
    public class EstudiantesController : Controller
    {
        private const string FilePath = "dataEstudiantes.txt";

        public EstudiantesController()
        {
            if (!System.IO.File.Exists(FilePath)) // Verificar si el archivo existe, si no, crearlo y escribir los datos iniciales
            {
                var Estudiantes = RecuperaEstudiante();
                EscribirDatosEnArchivo(Estudiantes);
            }
        }

        // GET: EstudiantesController
        public ActionResult Index()
        {
            var Estudiantes = LeerDatosDesdeArchivo();
            return View(Estudiantes);
        }

        // GET: EstudiantesController/Details/5
        public ActionResult Details(int idEstudiante)
        {
            var Estudiantes = LeerDatosDesdeArchivo();
            var Estudiante = Estudiantes.FirstOrDefault(e => e.idEstudiante == idEstudiante);
            return View(Estudiante);
        }

        // GET: EstudiantesController/Edit/5
        public ActionResult Edit(int idEstudiante)
        {
            var Estudiantes = LeerDatosDesdeArchivo();
            var Estudiante = Estudiantes.FirstOrDefault(e => e.idEstudiante == idEstudiante);
            return View(Estudiante);
        }

        // POST: EstudiantesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int idEstudiante, Estudiante editEstudiante)
        {
            try
            {
                var Estudiantes = LeerDatosDesdeArchivo();
                var Estudiante = Estudiantes.FirstOrDefault(e => e.idEstudiante == idEstudiante);

                if (Estudiante != null)
                {
                    Estudiante.Nombre = editEstudiante.Nombre;
                    Estudiante.ApelPaterno = editEstudiante.ApelPaterno;
                    Estudiante.ApelMaterno = editEstudiante.ApelMaterno;
                    Estudiante.FechaInscrip = editEstudiante.FechaInscrip;
                    Estudiante.Edad = editEstudiante.Edad;

                    EscribirDatosEnArchivo(Estudiantes);
                }

                return RedirectToAction(nameof(Index)); // Redirige a Index después de editar
            }
            catch
            {
                return View();
            }
        }

        // GET: EstudiantesController/Delete/5
        public ActionResult Delete(int idEstudiante)
        {
            var Estudiantes = LeerDatosDesdeArchivo();
            var Estudiante = Estudiantes.FirstOrDefault(e => e.idEstudiante == idEstudiante);
            return View(Estudiante);
        }

        // POST: EstudiantesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int idEstudiante, Estudiante deleteEstudiante)
        {
            try
            {
                var Estudiantes = LeerDatosDesdeArchivo();
                var Estudiante = Estudiantes.FirstOrDefault(e => e.idEstudiante == idEstudiante);

                if (Estudiante != null)
                {
                    // Remover completamente al estudiante de la lista
                    Estudiantes.Remove(Estudiante);

                    // Reescribir todos los estudiantes restantes en el archivo, excepto el que se eliminó
                    EscribirDatosEnArchivo(Estudiantes);
                }

                return RedirectToAction(nameof(Index)); // Redirige a Index después de eliminar
            }
            catch
            {
                return View();
            }
        }

        // GET: EstudiantesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EstudiantesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Estudiante estudiante)
        {
            try
            {
                var Estudiantes = LeerDatosDesdeArchivo();
                estudiante.idEstudiante = Estudiantes.Count + 1;
                Estudiantes.Add(estudiante);
                EscribirDatosEnArchivo(Estudiantes);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [NonAction]
        public List<Estudiante> LeerDatosDesdeArchivo()
        {
            var Estudiantes = new List<Estudiante>();
            using (var reader = new StreamReader(FilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var datos = line.Split(',');
                    var estudiante = new Estudiante
                    {
                        idEstudiante = int.Parse(datos[0]),
                        Nombre = datos[1],
                        ApelPaterno = datos[2],
                        ApelMaterno = datos[3],
                        FechaInscrip = DateTime.Parse(datos[4]),
                        Edad = int.Parse(datos[5])
                    };
                    Estudiantes.Add(estudiante);
                }
            }
            return Estudiantes;
        }

        [NonAction]
        public void EscribirDatosEnArchivo(List<Estudiante> estudiantes)
        {
            using (var writer = new StreamWriter(FilePath))
            {
                foreach (var estudiante in estudiantes)
                {
                    writer.WriteLine($"{estudiante.idEstudiante},{estudiante.Nombre},{estudiante.ApelPaterno},{estudiante.ApelMaterno},{estudiante.FechaInscrip},{estudiante.Edad}");
                }
            }
        }

        [NonAction]
        public List<Estudiante> RecuperaEstudiante()
        {
            return new List<Estudiante>
            {
                new Estudiante
                {
                    idEstudiante=1,
                    Nombre="Carlos",
                    ApelPaterno="Montoya",
                    ApelMaterno="Figueroa",
                    FechaInscrip=DateTime.Today,
                    Edad = 20
                },
                new Estudiante
                {
                    idEstudiante=2,
                    Nombre="Lourdes",
                    ApelPaterno="Peña",
                    ApelMaterno="Ardón",
                    FechaInscrip=DateTime.Today,
                    Edad = 18
                },
            };
        }
    }
}