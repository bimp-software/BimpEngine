using BimpEngine.Engine.Core.Interface;
using BimpEngine.Engine.Entities.Primitive;
using BimpEngine.Engine.World;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BimpEngine.Controls.Inspector.Componentes
{
    public partial class FurnitureControl : UserControl, IInspectorComponent
    {
        private Objetos objeto;
        public event Action<Objetos, Objetos> OnChildCreated; 

        public FurnitureControl()
        {
            InitializeComponent();

            btnLimpiar.Click += (s, e) => LimpiarHijos();
            btnGenerar.Click += (s, e) => GenerarMueble();
        }

        private void GenerarMueble()
        {
            if (objeto == null) return;

            LimpiarHijos();

            // 1 unidad = 1 metro, cubo base mide 2 unidades → dividir por 100
            double ancho = (double)nudAncho.Value / 100.0;
            double alto = (double)nudAlto.Value / 100.0;
            double fondo = (double)nudProfundidad.Value / 100.0;
            double grosor = (double)nudGrosorTabla.Value / 100.0;

            // El cubo base ya mide 2 unidades, escalar a la mitad para compensar
            objeto.Transform.Scale.X = ancho / 2.0;
            objeto.Transform.Scale.Y = alto / 2.0;
            objeto.Transform.Scale.Z = fondo / 2.0;

            objeto.MeshRenderer.Material.BlueprintMode = true;

            int cajones = (int)nudCajones.Value;
            int filas = (int)nudFilasCajones.Value;

            if (cajones > 0 && filas > 0)
                GenerarCajones(cajones, filas, ancho, alto, fondo, grosor);

            if (chkPuertaIzq.Checked)
                GenerarPuerta("Puerta Izq", -1, ancho, alto, fondo, grosor);

            if (chkPuertaDer.Checked)
                GenerarPuerta("Puerta Der", 1, ancho, alto, fondo, grosor);

            if (chkRepisas.Checked)
                GenerarRepisas((int)nudRepisas.Value, ancho, alto, fondo, grosor);
        }

        private void GenerarCajones(int columnas, int filas, double ancho, double alto, double fondo, double grosor)
        {
            double anchoC = (ancho - grosor * 2) / columnas;
            double altoC = (alto - grosor * 2) / filas;
            double profC = grosor * 0.5;

            for (int f = 0; f < filas; f++)
            {
                for (int c = 0; c < columnas; c++)
                {
                    double x = -ancho / 2 + grosor + anchoC * c + anchoC / 2;
                    double y = -alto / 2 + grosor + altoC * f + altoC / 2;
                    double z = fondo / 2 + profC / 2;

                    CrearHijo($"Cajon.{f:D3}{c:D3}", x, y, z,
                        anchoC - grosor * 0.1,
                        altoC - grosor * 0.1,
                        profC);
                }
            }
        }

        private void GenerarPuerta(string nombre, int lado, double ancho, double alto, double fondo, double grosor)
        {
            // Cada puerta ocupa la mitad del ancho interior, con grosor real de tabla
            double anchoPuerta = (ancho - grosor * 3) / 2.0; // mitad del interior
            double altoPuerta = alto - grosor * 2;            // alto interior completo
            double profPuerta = grosor;                       // grosor real de una tabla

            double x = lado * (anchoPuerta / 2.0 + grosor / 2.0);
            double y = 0;
            double z = fondo / 2.0 + profPuerta / 2.0;      // sobresale al frente

            CrearHijo(nombre, x, y, z, anchoPuerta, altoPuerta, profPuerta);
        }

        private void GenerarRepisas(int cantidad, double ancho, double alto, double fondo, double grosor)
        {
            double espacioY = (alto - grosor * 2) / (cantidad + 1);
            // Grosor visual de repisa = al menos el grosor de tabla, mínimo 2 cm
            double grosorRepisa = System.Math.Max(grosor, 0.02);

            for (int i = 1; i <= cantidad; i++)
            {
                double y = -alto / 2 + grosor + espacioY * i;
                CrearHijo($"Repisa.{i:D3}", 0, y, 0,
                    ancho - grosor * 2,
                    grosorRepisa,
                    fondo - grosor * 2);
            }
        }

        private void CrearHijo(string nombre, double x, double y, double z, double sx, double sy, double sz)
        {
            var hijo = new Cube();
            hijo.Name = nombre;
            hijo.Transform.Position.X = x;
            hijo.Transform.Position.Y = y;
            hijo.Transform.Position.Z = z;

            hijo.Transform.Scale.X = sx / 2.0;
            hijo.Transform.Scale.Y = sy / 2.0;
            hijo.Transform.Scale.Z = sz / 2.0;

            hijo.MeshRenderer.Material.BlueprintMode = true;

            objeto.AddChild(hijo);
            OnChildCreated?.Invoke(hijo, objeto);
        }

        private void LimpiarHijos()
        {
            if (objeto == null) return;
            foreach (var hijo in objeto.Children.ToList())
                objeto.RemoveChild(hijo);
        }

        public void SetObject(Objetos obj)
        {
            objeto = obj;
            Refresh(obj);
        }

        public void Refresh(Objetos obj)
        {
            objeto = obj;
        }

    }
}
