using System;
using System.Threading;
using System.Drawing;
using System.Drawing.Imaging;
namespace RedimensionadorConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("------------------Redimensionador de Imagens------------------");

            //Cria uma instância da classe Thread com o método ResizeImageProcess
            Thread thread1 = new Thread(ResizeImageProcess);

            //Inicia o thread1
            thread1.Start();

            Console.WriteLine("O redimensionamento segue como base a altura: 600, 720, 900 e 1080.");
            Console.WriteLine("Insira os arquivos a serem redimensionados na pasta Entry_Files.");
            Console.WriteLine("Os arquivos redimensionados vão para a pasta Resized_Files. \n"
                + "Após o processo os arquivos da pasta Entry_Files são transferidos "
                + "para a pasta Finished_Files. \n"
                + "Aguardando arquivos na pasta Entry_files...");
            Console.WriteLine("Ao finalizar, feche a janela.");
        }
        static void ResizeImageProcess()
        {
            
            #region "Directories"
            // Criação dos diretórios, caso não exista
            string entryFiles = "Entry_Files";
            string finishedFiles = "Finished_Files";
            string resizedFiles = "Resized_Files";
            if (!Directory.Exists(entryFiles))
            {
                Directory.CreateDirectory(entryFiles);
            }
            if (!Directory.Exists(finishedFiles))
            {
                Directory.CreateDirectory(finishedFiles);
            }
            if (!Directory.Exists(resizedFiles)) 
            {
                Directory.CreateDirectory(resizedFiles);
            }
            // Definição do tamanho das alturas que serão base para o redimensionamento
            int imageHeight1 = 600;
            int imageHeight2 = 720;
            int imageHeight3 = 900;
            int imageHeight4 = 1080;

            // Criação dos objetos das classes FileStream e FileInfo
            FileStream fileStream;
            FileInfo fileInfo;
            #endregion
            while (true)
            {
                /*Esse while ficará ativo.
                Se tiver arquivo, irá redimensionar
                Copiará o arquivo para a pasta resized
                Depois de redimensionar, enviará os aqruivos da pasta 
                entrypara a pasta finished*/

                var entry = Directory.EnumerateFiles(entryFiles);
                /*coloca um enum com o nome completo dos arquivos(seu endereço) da pasta Entry Files
                na var implícita entry*/
                
                /* Esse foreach percorre a var entry em que estão listados os arquivos,
                 * na prática, percorrendo cada arquivo e executando as ações 
                */
                foreach (var file in entry)
                {
                    /* Instancia as classes FileStream e FileInfo nos objetos já criados,
                     * não sobrecarregando a memória
                     */
                    fileStream = new FileStream(file, FileMode.Open
                        , FileAccess.ReadWrite, FileShare.ReadWrite);
                    fileInfo = new FileInfo(file);

                    // Definição do caminho e nome do arquivo para cada tamanho especificado
                    string fullPathResized1 = Environment.CurrentDirectory + @"\" + resizedFiles
                        + @"\" + 600 + "_" + fileInfo.Name;
                    string fullPathResized2 = Environment.CurrentDirectory + @"\" + resizedFiles
                        + @"\" + 720 + "_" + fileInfo.Name;
                    string fullPathResized3 = Environment.CurrentDirectory + @"\" + resizedFiles
                        + @"\" + 900 + "_" + fileInfo.Name;
                    string fullPathResized4 = Environment.CurrentDirectory + @"\" + resizedFiles
                        + @"\" + 1080 + "_" + fileInfo.Name;

                    // Chama o método ResizeImage para cada tamanho
                    ResizeImage(Image.FromStream(fileStream), imageHeight1, fullPathResized1);
                    ResizeImage(Image.FromStream(fileStream), imageHeight2, fullPathResized2);
                    ResizeImage(Image.FromStream(fileStream), imageHeight3, fullPathResized3);
                    ResizeImage(Image.FromStream(fileStream), imageHeight4, fullPathResized4);

                    // Fecha a instância do objeto fileStream e libera recursos do sistema
                    fileStream.Close();

                    // Define o caminho e nome dos arquivos finalizados (os arquivos da pasta Entry_Files)
                    string fullPathFinished = Environment.CurrentDirectory + @"\" + finishedFiles
                        + @"\" + fileInfo.Name;

                    // Move os arquivos da pasta Entry_Files para a pasta Finished_Files
                    fileInfo.MoveTo(fullPathFinished);

                }
                // O thread irá descansar a cada 4 segundos, após esse tempo ele executa novamente
                Thread.Sleep(new TimeSpan(0, 0, 4));
            }
        }
        /// <summary>
        /// Faz o redimensionamento de imagem
        /// </summary>
        /// <param name="image">imagem a ser redimensionada</param>
        /// <param name="height">altura desejada para a imagem</param>
        /// <param name="path">aonde será gravado o arquivo</param>
        /// <returns></returns>
        static void ResizeImage(Image image, int height, string path)
        {
            // Aspect ratio baseado na altura desejada
            double ratio = (double) height / image.Height;

            // A nova largura e altura são proporcionais ao ratio
            int newWidth = Convert.ToInt32(ratio * image.Width);
            int newHeight = Convert.ToInt32(ratio * image.Height);

            // Instancia um novo Bitmap com os novos tamanhos
            Bitmap newImage = new Bitmap(newWidth, newHeight);

            // Usa o Graphics para manter a qualidade de imagem e fazer o redimensionamento
            using (Graphics g = Graphics.FromImage(newImage))
            {
                /* Chama o método DrawImage do Graphics, inserindo a imagem,
                 * os parâmetros de localização na tela e os novos tamanhos
                */
                g.DrawImage(image, 0, 0, newWidth, newHeight);
            }

            // Salva a imagem no caminho indicado
            newImage.Save(path);

            // Dispose indica que já não é mais preciso guardar a imagem na memória
            image.Dispose();
        }
    }
}
