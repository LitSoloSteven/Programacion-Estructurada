using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;


namespace GestorEmpleadosWinForms
{
    // Esta es la clase principal de nuestro formulario (la ventana).
    public class Gestor_Formulario_Principal : Form
    {
        //  Declaración de todos los Controles Visuales
        // Aquí declaramos cada botón, campo de texto, etc., que usaremos en la ventana.

        // Sección para seleccionar la carpeta
        private TextBox Campo_Texto_Ruta_Carpeta;
        private Button Boton_Buscar_Carpeta;

        // Sección para mostrar la lista de archivos
        private ListBox Lista_Archivos_Empleados;

        // Sección con los datos del empleado
        private TextBox Campo_Texto_Nombre;
        private TextBox Campo_Texto_Edad;
        private TextBox Campo_Texto_Salario;
        private ComboBox Menu_Desplegable_Sexo;

        // Sección con los botones de acción
        private Button Boton_Crear_Nuevo;
        private Button Boton_Actualizar_Seleccionado;
        private Button Boton_Eliminar_Seleccionado;
        private Button Boton_Limpiar_Campos;

        // Etiqueta en la parte inferior para mostrar mensajes al usuario
        private Label Etiqueta_Estado;

        // Variable para guardar la ruta de la carpeta que elija el usuario.
        private string rutaDeLaCarpetaSeleccionada = "";

        //  Constructor del Formulario 
        // Este es el primer método que se ejecuta cuando se crea la ventana.
        public Gestor_Formulario_Principal()
        {
            // Configuramos las propiedades básicas de la ventana.
            this.Text = "Gestor de Empleados"; // Título de la ventana
            this.Size = new Size(600, 480);     // Tamaño inicial
            this.MinimumSize = new Size(600, 480); // Tamaño mínimo
            this.StartPosition = FormStartPosition.CenterScreen; // Aparece en el centro
            this.Font = new Font("Segoe UI", 9F); // Tipo de letra para toda la app

            // Usamos la cultura "en-US" para que el formato de moneda (símbolo $) funcione correctamente.
            CultureInfo.CurrentCulture = new CultureInfo("en-US", false);
            
            // Llamamos al método que se encargará de crear y dibujar todos los controles.
            Inicializar_Componentes_Visuales();
        }

        //  Creación de la Interfaz Gráfica 
        // Este método organiza la creación de todos los elementos visuales.
        private void Inicializar_Componentes_Visuales()
        {
            //  Creamos la sección para seleccionar la carpeta de trabajo
            var grupoCarpeta = new GroupBox { Text = "Carpeta de Trabajo", Location = new Point(15, 15), Size = new Size(550, 60) };
            
            Campo_Texto_Ruta_Carpeta = new TextBox { Location = new Point(15, 25), Width = 420, ReadOnly = true, BorderStyle = BorderStyle.FixedSingle };
            Boton_Buscar_Carpeta = new Button { Text = "Seleccionar...", Location = new Point(445, 23), Size = new Size(90, 23) };
            
            // Le decimos al botón qué hacer cuando le den clic.
            Boton_Buscar_Carpeta.Click += Al_Hacer_Clic_En_Boton_Buscar_Carpeta;
            
            grupoCarpeta.Controls.Add(Campo_Texto_Ruta_Carpeta);
            grupoCarpeta.Controls.Add(Boton_Buscar_Carpeta);
            this.Controls.Add(grupoCarpeta);
            
            //   Creamos la lista donde se mostrarán los archivos .txt 
            Lista_Archivos_Empleados = new ListBox { Location = new Point(15, 85), Size = new Size(220, 330) };
            
            // Le decimos a la lista qué hacer cuando el usuario seleccione un archivo.
            Lista_Archivos_Empleados.SelectedIndexChanged += Al_Cambiar_Seleccion_En_Lista_Archivos;
            
            this.Controls.Add(Lista_Archivos_Empleados);

            //   Creamos el grupo para los datos del empleado 
            var grupoDatos = new GroupBox { Text = "Datos del Empleado", Location = new Point(245, 85), Size = new Size(320, 180) };
            
            Campo_Texto_Nombre = new TextBox { Location = new Point(80, 30), Width = 225, BorderStyle = BorderStyle.FixedSingle };
            Campo_Texto_Edad = new TextBox { Location = new Point(80, 60), Width = 225, BorderStyle = BorderStyle.FixedSingle };
            Campo_Texto_Salario = new TextBox { Location = new Point(80, 90), Width = 225, BorderStyle = BorderStyle.FixedSingle };
            Menu_Desplegable_Sexo = new ComboBox { Location = new Point(80, 120), Width = 225, DropDownStyle = ComboBoxStyle.DropDownList };
            Menu_Desplegable_Sexo.Items.AddRange(new object[] { "M", "F" }); // Añadimos las opciones

            // Añadimos las etiquetas de texto (Label) y les ponemos la propiedad AutoSize en true para evitar problemas visuales.
            grupoDatos.Controls.Add(new Label { Text = "Nombre:", Location = new Point(15, 33), AutoSize = true });
            grupoDatos.Controls.Add(Campo_Texto_Nombre);
            grupoDatos.Controls.Add(new Label { Text = "Edad:", Location = new Point(15, 63), AutoSize = true });
            grupoDatos.Controls.Add(Campo_Texto_Edad);
            grupoDatos.Controls.Add(new Label { Text = "Salario:", Location = new Point(15, 93), AutoSize = true });
            grupoDatos.Controls.Add(Campo_Texto_Salario);
            grupoDatos.Controls.Add(new Label { Text = "Sexo:", Location = new Point(15, 123), AutoSize = true });
            grupoDatos.Controls.Add(Menu_Desplegable_Sexo);
            this.Controls.Add(grupoDatos);

            //  Creamos los botones de acción (Crear, Actualizar, etc.) 
            Boton_Crear_Nuevo = new Button { Text = "Crear Nuevo", Location = new Point(245, 280), Size = new Size(155, 30) };
            Boton_Actualizar_Seleccionado = new Button { Text = "Actualizar Seleccionado", Location = new Point(410, 280), Size = new Size(155, 30) };
            Boton_Eliminar_Seleccionado = new Button { Text = "Eliminar Seleccionado", Location = new Point(410, 320), Size = new Size(155, 30) };
            Boton_Limpiar_Campos = new Button { Text = "Limpiar Campos", Location = new Point(245, 320), Size = new Size(155, 30) };

            // Asignamos a cada botón la función que debe ejecutar al hacerle clic.
            Boton_Crear_Nuevo.Click += Al_Hacer_Clic_En_Boton_Crear;
            Boton_Actualizar_Seleccionado.Click += Al_Hacer_Clic_En_Boton_Actualizar;
            Boton_Eliminar_Seleccionado.Click += Al_Hacer_Clic_En_Boton_Eliminar;
            Boton_Limpiar_Campos.Click += (sender, e) => Limpiar_Y_Reiniciar_Campos(false);

            this.Controls.Add(Boton_Crear_Nuevo);
            this.Controls.Add(Boton_Actualizar_Seleccionado);
            this.Controls.Add(Boton_Eliminar_Seleccionado);
            this.Controls.Add(Boton_Limpiar_Campos);

            //  Creamos la etiqueta de estado en la parte inferior 
            Etiqueta_Estado = new Label { Text = "Selecciona una carpeta para empezar.", Location = new Point(15, 425), ForeColor = Color.Blue, AutoSize = true };
            this.Controls.Add(Etiqueta_Estado);

            // Dejamos la interfaz en su estado inicial (campos limpios, botones desactivados).
            Limpiar_Y_Reiniciar_Campos(true);
        }

        //  Lógica de los Eventos (Qué hacer cuando el usuario interactúa) 

        // Se ejecuta cuando el usuario hace clic en el botón "Seleccionar...".
        private void Al_Hacer_Clic_En_Boton_Buscar_Carpeta(object sender, EventArgs e)
        {
            // Usamos un diálogo estándar de Windows para que el usuario elija una carpeta.
            using (var dialogoCarpeta = new FolderBrowserDialog())
            {
                dialogoCarpeta.Description = "Selecciona la carpeta para guardar los registros";
                
                // Si el usuario elige una carpeta y presiona OK...
                if (dialogoCarpeta.ShowDialog() == DialogResult.OK)
                {
                    // Guardamos la ruta de la carpeta.
                    rutaDeLaCarpetaSeleccionada = dialogoCarpeta.SelectedPath;
                    Campo_Texto_Ruta_Carpeta.Text = rutaDeLaCarpetaSeleccionada;
                    
                    // Actualizamos la lista de archivos y mostramos un mensaje de éxito.
                    Actualizar_Lista_De_Archivos();
                    Etiqueta_Estado.Text = $"Carpeta '{Path.GetFileName(rutaDeLaCarpetaSeleccionada)}' seleccionada.";
                    Etiqueta_Estado.ForeColor = Color.Green;
                }
            }
        }
        
        // Se ejecuta cuando el usuario selecciona un elemento diferente en la lista de archivos.
        private void Al_Cambiar_Seleccion_En_Lista_Archivos(object sender, EventArgs e)
        {
            // Si no hay nada seleccionado, no hacemos nada.
            if (Lista_Archivos_Empleados.SelectedItem == null) return;

            // Obtenemos el nombre del archivo y construimos la ruta completa.
            string nombreArchivo = Lista_Archivos_Empleados.SelectedItem.ToString();
            string rutaCompleta = Path.Combine(rutaDeLaCarpetaSeleccionada, nombreArchivo);
            
            // Cargamos los datos de ese archivo en los campos de texto.
            Cargar_Datos_Desde_Archivo(rutaCompleta);
        }

        // Se ejecuta al hacer clic en el botón "Crear Nuevo".
        private void Al_Hacer_Clic_En_Boton_Crear(object sender, EventArgs e)
        {
            // Primero, validamos que se haya seleccionado una carpeta.
            if (string.IsNullOrEmpty(rutaDeLaCarpetaSeleccionada))
            {
                MessageBox.Show("Por favor, selecciona primero una carpeta de trabajo.", "Carpeta no seleccionada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Luego, validamos que los datos ingresados sean correctos.
            if (!Son_Validos_Los_Campos()) return;
            
            // Mostramos un diálogo para que el usuario elija el nombre del archivo a guardar.
            using (var dialogoGuardar = new SaveFileDialog())
            {
                dialogoGuardar.InitialDirectory = rutaDeLaCarpetaSeleccionada;
                dialogoGuardar.Filter = "Archivo de texto (*.txt)|*.txt";
                dialogoGuardar.Title = "Asignar nombre al nuevo registro";
                dialogoGuardar.FileName = "nuevo_empleado.txt";

                // Si el usuario elige un nombre y presiona "Guardar"...
                if (dialogoGuardar.ShowDialog() == DialogResult.OK)
                {
                    Guardar_Datos_En_Archivo(dialogoGuardar.FileName);
                    Etiqueta_Estado.Text = $"Archivo '{Path.GetFileName(dialogoGuardar.FileName)}' creado.";
                    Etiqueta_Estado.ForeColor = Color.Green;
                    Actualizar_Lista_De_Archivos();
                }
            }
        }

        // Se ejecuta al hacer clic en el botón "Actualizar Seleccionado".
        private void Al_Hacer_Clic_En_Boton_Actualizar(object sender, EventArgs e)
        {
            // Validamos que haya un archivo seleccionado en la lista.
            if (Lista_Archivos_Empleados.SelectedItem == null)
            {
                MessageBox.Show("Por favor, selecciona un archivo de la lista para actualizar.", "Ningún archivo seleccionado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Validamos que los datos en los campos sean correctos.
            if (!Son_Validos_Los_Campos()) return;
            
            // Construimos la ruta del archivo y guardamos los cambios.
            string rutaCompleta = Path.Combine(rutaDeLaCarpetaSeleccionada, Lista_Archivos_Empleados.SelectedItem.ToString());
            Guardar_Datos_En_Archivo(rutaCompleta);
            Etiqueta_Estado.Text = $"Archivo '{Path.GetFileName(rutaCompleta)}' actualizado.";
            Etiqueta_Estado.ForeColor = Color.Green;
        }

        // Se ejecuta al hacer clic en el botón "Eliminar Seleccionado".
        private void Al_Hacer_Clic_En_Boton_Eliminar(object sender, EventArgs e)
        {
            if (Lista_Archivos_Empleados.SelectedItem == null)
            {
                MessageBox.Show("Por favor, selecciona un archivo de la lista para eliminar.", "Ningún archivo seleccionado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string nombreArchivo = Lista_Archivos_Empleados.SelectedItem.ToString();
            string rutaCompleta = Path.Combine(rutaDeLaCarpetaSeleccionada, nombreArchivo);
            
            // Pedimos confirmación al usuario antes de borrar.
            var confirmacion = MessageBox.Show($"¿Estás seguro de que quieres eliminar '{nombreArchivo}'?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmacion == DialogResult.Yes)
            {
                try
                {
                    File.Delete(rutaCompleta);
                    Etiqueta_Estado.Text = $"Archivo '{nombreArchivo}' eliminado.";
                    Etiqueta_Estado.ForeColor = Color.OrangeRed;
                    Actualizar_Lista_De_Archivos();
                    Limpiar_Y_Reiniciar_Campos(false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar el archivo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        
        //  Funciones de Ayuda (Lógica interna del programa) 

        // Lee todos los archivos .txt de la carpeta seleccionada y los muestra en la lista.
        private void Actualizar_Lista_De_Archivos()
        {
            Lista_Archivos_Empleados.Items.Clear(); // Limpiamos la lista actual.
            if (!string.IsNullOrEmpty(rutaDeLaCarpetaSeleccionada) && Directory.Exists(rutaDeLaCarpetaSeleccionada))
            {
                var archivos = Directory.GetFiles(rutaDeLaCarpetaSeleccionada, "*.txt")
                                        .Select(Path.GetFileName)
                                        .ToArray();
                Lista_Archivos_Empleados.Items.AddRange(archivos);
            }
            Limpiar_Y_Reiniciar_Campos(false);
        }

        // Lee un archivo de texto y pone los datos en los campos correspondientes.
        private void Cargar_Datos_Desde_Archivo(string rutaDelArchivo)
        {
            try
            {
                var lineasDelArchivo = File.ReadAllLines(rutaDelArchivo);
                var diccionarioDeDatos = new Dictionary<string, string>();
                
                // Leemos cada línea del archivo (ej: "Nombre: Juan")
                foreach (var linea in lineasDelArchivo)
                {
                    var partes = linea.Split(new[] { ':' }, 2); // Separamos por los dos puntos ":"
                    if (partes.Length == 2)
                    {
                        // Guardamos la clave ("nombre") y el valor ("Juan") en el diccionario.
                        diccionarioDeDatos[partes[0].Trim().ToLower()] = partes[1].Trim();
                    }
                }

                //  Aquí está la lógica que reemplaza la "clase de extensión" 
                // Para cada campo, comprobamos si la clave existe en el diccionario.
                // Si existe, usamos su valor. Si no, usamos un valor por defecto (ej. texto vacío).
                // Esto evita que el programa se cierre si un archivo está incompleto.
                
                Campo_Texto_Nombre.Text = diccionarioDeDatos.ContainsKey("nombre") ? diccionarioDeDatos["nombre"] : "";
                Campo_Texto_Edad.Text = diccionarioDeDatos.ContainsKey("edad") ? diccionarioDeDatos["edad"] : "";
                
                string salarioTexto = diccionarioDeDatos.ContainsKey("salario") ? diccionarioDeDatos["salario"] : "0";
                Campo_Texto_Salario.Text = decimal.Parse(salarioTexto, NumberStyles.Currency).ToString(); // Le quitamos el formato de moneda.
                
                Menu_Desplegable_Sexo.SelectedItem = diccionarioDeDatos.ContainsKey("sexo") ? diccionarioDeDatos["sexo"] : null;
                
                // Activamos los botones de Actualizar y Eliminar.
                Boton_Actualizar_Seleccionado.Enabled = true;
                Boton_Eliminar_Seleccionado.Enabled = true;
                Etiqueta_Estado.Text = $"Mostrando datos de '{Path.GetFileName(rutaDelArchivo)}'.";
                Etiqueta_Estado.ForeColor = Color.DarkBlue;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al leer el archivo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Limpiar_Y_Reiniciar_Campos(false);
            }
        }

        // Guarda la información de los campos de texto en un archivo.
        private void Guardar_Datos_En_Archivo(string rutaDelArchivo)
        {
            try
            {
                // Creamos el texto que se guardará en el archivo.
                string contenido = $"Nombre: {Campo_Texto_Nombre.Text}\n" +
                                   $"Edad: {Campo_Texto_Edad.Text}\n" +
                                   $"Salario: {decimal.Parse(Campo_Texto_Salario.Text):C}\n" + // :C le da formato de moneda
                                   $"Sexo: {Menu_Desplegable_Sexo.SelectedItem}\n";
                
                File.WriteAllText(rutaDelArchivo, contenido);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el archivo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        // Limpia todos los campos de datos y reinicia los botones.
        private void Limpiar_Y_Reiniciar_Campos(bool limpiarTodoElPrograma)
        {
            // Si es un reinicio completo, también limpiamos la carpeta seleccionada.
            if (limpiarTodoElPrograma)
            {
                rutaDeLaCarpetaSeleccionada = "";
                Campo_Texto_Ruta_Carpeta.Text = "";
                Lista_Archivos_Empleados.Items.Clear();
                Etiqueta_Estado.Text = "Selecciona una carpeta para empezar.";
                Etiqueta_Estado.ForeColor = Color.Blue;
            }
            
            // Limpiamos los campos de datos del empleado.
            Lista_Archivos_Empleados.ClearSelected();
            Campo_Texto_Nombre.Text = "";
            Campo_Texto_Edad.Text = "";
            Campo_Texto_Salario.Text = "";
            Menu_Desplegable_Sexo.SelectedIndex = -1;
            
            // Desactivamos los botones que no se pueden usar sin un archivo seleccionado.
            Boton_Actualizar_Seleccionado.Enabled = false;
            Boton_Eliminar_Seleccionado.Enabled = false;
            
            // Ponemos el cursor en el campo de nombre para que el usuario pueda escribir.
            Campo_Texto_Nombre.Focus();
        }

        // Verifica que los datos ingresados por el usuario sean válidos.
        private bool Son_Validos_Los_Campos()
        {
            if (string.IsNullOrWhiteSpace(Campo_Texto_Nombre.Text) || Menu_Desplegable_Sexo.SelectedItem == null)
            {
                MessageBox.Show("El nombre y el sexo no pueden estar vacíos.", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!int.TryParse(Campo_Texto_Edad.Text, out int edad) || edad <= 0)
            {
                MessageBox.Show("La edad debe ser un número entero positivo.", "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!decimal.TryParse(Campo_Texto_Salario.Text, out decimal salario) || salario < 0)
            {
                MessageBox.Show("El salario debe ser un número válido.", "Dato inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true; // Si todo está bien, devuelve verdadero.
        }

        //  Punto de Entrada de la Aplicación 
        // Este es el método `Main` que inicia todo el programa.
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Creamos y ejecutamos nuestro formulario principal.
            Application.Run(new Gestor_Formulario_Principal());
        }
    }

}

