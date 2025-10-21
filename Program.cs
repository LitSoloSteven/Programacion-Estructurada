// "Librerías" necesarias para crear la ventana, manejar archivos y datos.
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

// Espacio de nombres para organizar el código de esta aplicación.
namespace AnalizadorDeRegistros
{
    // --- Estructura para almacenar los datos de un empleado ---
    // Usamos una 'struct' porque es una forma simple y eficiente de agrupar
    // varias variables relacionadas (nombre, edad, etc.) en un solo paquete.
    public struct Empleado
    {
        public string Nombre { get; set; }
        public int Edad { get; set; }
        public decimal Salario { get; set; }
        public char Sexo { get; set; }
    }

    // --- Clase principal de nuestro formulario (la ventana) ---
    public class Analizador_Formulario_Principal : Form
    {
        // --- Declaración de todos los Controles Visuales ---
        private Button Boton_Seleccionar_Carpeta;
        private TextBox Campo_Texto_Ruta_Carpeta;
        
        // La tabla (DataGridView) para mostrar los datos de los empleados.
        private DataGridView Tabla_Resultados_Empleados;

        // Las etiquetas (Label) para mostrar las estadísticas.
        private Label Etiqueta_Promedio_Edad;
        private Label Etiqueta_Promedio_Salario;
        private Label Etiqueta_Conteo_Hombres;
        private Label Etiqueta_Conteo_Mujeres;

        // --- Constructor del Formulario ---
        // Se ejecuta al crear la ventana.
        public Analizador_Formulario_Principal()
        {
            // Configuración básica de la ventana.
            this.Text = "Analizador de Registros de Empleados";
            this.Size = new Size(700, 500);
            this.MinimumSize = new Size(700, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9F);

            // ** CAMBIO IMPORTANTE **
            // Usamos la cultura "en-US" para poder LEER correctamente el formato de moneda ($) de los archivos generados por el otro programa.
            CultureInfo.CurrentCulture = new CultureInfo("en-US", false);
            
            // Llamada al método que crea y dibuja la interfaz.
            Inicializar_Componentes_Visuales();
        }

        // --- Creación de la Interfaz Gráfica ---
        private void Inicializar_Componentes_Visuales()
        {
            // --- 1. Sección superior para seleccionar la carpeta ---
            var panelSuperior = new Panel { Dock = DockStyle.Top, Height = 50, Padding = new Padding(10) };
            
            Boton_Seleccionar_Carpeta = new Button { Text = "Seleccionar Carpeta con Registros", Dock = DockStyle.Left, Width = 220 };
            Boton_Seleccionar_Carpeta.Click += Al_Hacer_Clic_En_Boton_Seleccionar_Carpeta;
            
            Campo_Texto_Ruta_Carpeta = new TextBox { Dock = DockStyle.Fill, ReadOnly = true, Margin = new Padding(5, 0, 0, 0), BorderStyle = BorderStyle.FixedSingle };
            
            panelSuperior.Controls.Add(Campo_Texto_Ruta_Carpeta);
            panelSuperior.Controls.Add(Boton_Seleccionar_Carpeta);
            
            // --- 2. Panel inferior para mostrar las estadísticas ---
            var panelInferior = new GroupBox { Text = "Estadísticas Generales", Dock = DockStyle.Bottom, Height = 80, Padding = new Padding(10) };
            
            Etiqueta_Promedio_Edad = new Label { Text = "Promedio de Edad: -", Location = new Point(20, 30), AutoSize = true, Font = new Font(this.Font, FontStyle.Bold) };
            Etiqueta_Promedio_Salario = new Label { Text = "Promedio de Salario: -", Location = new Point(220, 30), AutoSize = true, Font = new Font(this.Font, FontStyle.Bold) };
            Etiqueta_Conteo_Hombres = new Label { Text = "Hombres (M): -", Location = new Point(450, 30), AutoSize = true, Font = new Font(this.Font, FontStyle.Bold) };
            Etiqueta_Conteo_Mujeres = new Label { Text = "Mujeres (F): -", Location = new Point(580, 30), AutoSize = true, Font = new Font(this.Font, FontStyle.Bold) };

            panelInferior.Controls.Add(Etiqueta_Promedio_Edad);
            panelInferior.Controls.Add(Etiqueta_Promedio_Salario);
            panelInferior.Controls.Add(Etiqueta_Conteo_Hombres);
            panelInferior.Controls.Add(Etiqueta_Conteo_Mujeres);
            
            // --- 3. Tabla (Grid) para mostrar los datos de los empleados ---
            Tabla_Resultados_Empleados = new DataGridView
            {
                Dock = DockStyle.Fill, // Le decimos que ocupe todo el espacio sobrante.
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = SystemColors.Window,
                BorderStyle = BorderStyle.Fixed3D
            };

            // --- 4. **CORRECCIÓN IMPORTANTE**: Añadimos los controles a la ventana en el orden correcto ---
            // El control que se "llena" (Fill) se debe añadir antes que los paneles superior e inferior
            // para que estos se posicionen correctamente a su alrededor y no lo tapen.
            this.Controls.Add(this.Tabla_Resultados_Empleados);
            this.Controls.Add(panelInferior);
            this.Controls.Add(panelSuperior);
        }

        // --- Lógica de los Eventos ---

        // Se ejecuta cuando el usuario hace clic en el botón para seleccionar una carpeta.
        private void Al_Hacer_Clic_En_Boton_Seleccionar_Carpeta(object sender, EventArgs e)
        {
            using (var dialogoCarpeta = new FolderBrowserDialog())
            {
                dialogoCarpeta.Description = "Selecciona la carpeta que contiene los archivos de empleados (.txt)";
                if (dialogoCarpeta.ShowDialog() == DialogResult.OK)
                {
                    // Mostramos la ruta en el campo de texto.
                    string rutaCarpeta = dialogoCarpeta.SelectedPath;
                    Campo_Texto_Ruta_Carpeta.Text = rutaCarpeta;
                    
                    // Llamamos a la función principal que procesará los archivos.
                    Procesar_Archivos_De_La_Carpeta(rutaCarpeta);
                }
            }
        }

        // --- Funciones de Ayuda (Lógica principal del programa) ---

        // Esta es la función más importante: lee los archivos, calcula y muestra los resultados.
        private void Procesar_Archivos_De_La_Carpeta(string rutaCarpeta)
        {
            // 1. Buscamos todos los archivos que terminen en .txt dentro de la carpeta.
            string[] rutasDeArchivos = Directory.GetFiles(rutaCarpeta, "*.txt");
            
            // Si no encontramos archivos, mostramos un mensaje y terminamos.
            if (rutasDeArchivos.Length == 0)
            {
                MessageBox.Show("No se encontraron archivos .txt en la carpeta seleccionada.", "Sin Registros", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Limpiamos la tabla y las estadísticas si no hay archivos.
                Tabla_Resultados_Empleados.DataSource = null;
                Etiqueta_Promedio_Edad.Text = "Promedio de Edad: -";
                Etiqueta_Promedio_Salario.Text = "Promedio de Salario: -";
                Etiqueta_Conteo_Hombres.Text = "Hombres (M): -";
                Etiqueta_Conteo_Mujeres.Text = "Mujeres (F): -";
                return;
            }

            // 2. Creamos una lista para guardar los datos de todos los empleados que encontremos.
            var listaDeEmpleados = new List<Empleado>();

            // 3. Recorremos cada archivo encontrado.
            foreach (string rutaArchivo in rutasDeArchivos)
            {
                try
                {
                    // Leemos todas las líneas del archivo actual.
                    var lineas = File.ReadAllLines(rutaArchivo);
                    
                    // Creamos un empleado temporal para guardar sus datos.
                    var empleado = new Empleado();
                    
                    // Leemos cada línea ("Nombre: Juan", "Edad: 30", etc.).
                    foreach (var linea in lineas)
                    {
                        var partes = linea.Split(new[] { ':' }, 2);
                        if (partes.Length == 2)
                        {
                            string clave = partes[0].Trim().ToLower();
                            string valor = partes[1].Trim();

                            // Según la clave, guardamos el valor en la variable correspondiente.
                            switch (clave)
                            {
                                case "nombre":
                                    empleado.Nombre = valor;
                                    break;
                                case "edad":
                                    empleado.Edad = int.Parse(valor);
                                    break;
                                case "salario":
                                    empleado.Salario = decimal.Parse(valor, NumberStyles.Currency); // Le quitamos el formato de moneda ($).
                                    break;
                                case "sexo":
                                    empleado.Sexo = char.Parse(valor);
                                    break;
                            }
                        }
                    }
                    // Una vez leídos todos los datos del archivo, añadimos el empleado a nuestra lista.
                    listaDeEmpleados.Add(empleado);
                }
                catch (Exception ex)
                {
                    // Si un archivo está mal formateado, mostramos un aviso pero continuamos con los demás.
                    MessageBox.Show($"Error al procesar el archivo '{Path.GetFileName(rutaArchivo)}':\n{ex.Message}\n\nSe omitirá este archivo.", "Error de Formato", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            
            // 4. Si después de procesar todo, la lista tiene empleados, calculamos las estadísticas.
            if (listaDeEmpleados.Any())
            {
                // Mostramos los datos en la tabla.
                // Usamos 'ToList()' para crear una copia, lo que a veces ayuda a que la tabla se refresque mejor.
                Tabla_Resultados_Empleados.DataSource = listaDeEmpleados.ToList();
                
                // Calculamos promedios y conteos usando funciones de LINQ (son como atajos para listas).
                double promedioEdad = listaDeEmpleados.Average(emp => emp.Edad);
                decimal promedioSalario = listaDeEmpleados.Average(emp => emp.Salario);
                int conteoHombres = listaDeEmpleados.Count(emp => emp.Sexo == 'M');
                int conteoMujeres = listaDeEmpleados.Count(emp => emp.Sexo == 'F');

                // 5. Mostramos los resultados en las etiquetas.
                Etiqueta_Promedio_Edad.Text = $"Promedio de Edad: {promedioEdad:F1} años"; // F1 significa 1 decimal.
                
                // ** CAMBIO IMPORTANTE **
                // Para mostrar el salario en Córdobas (C$), creamos un objeto de cultura para Nicaragua.
                var culturaNicaragua = new CultureInfo("es-NI");
                Etiqueta_Promedio_Salario.Text = $"Promedio de Salario: {promedioSalario.ToString("C", culturaNicaragua)}"; 
                
                Etiqueta_Conteo_Hombres.Text = $"Hombres (M): {conteoHombres}";
                Etiqueta_Conteo_Mujeres.Text = $"Mujeres (F): {conteoMujeres}";
            }
        }
        
        // --- Punto de Entrada de la Aplicación ---
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Creamos y ejecutamos nuestro formulario principal.
            Application.Run(new Analizador_Formulario_Principal());
        }
    }
}