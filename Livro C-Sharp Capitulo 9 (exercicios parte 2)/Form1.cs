using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;

namespace Livro_C_Sharp_Capitulo_9__exercicios_parte_2_
{
    public partial class Form1 : Form
    {
        string ficheiroOriginal;
        string ficheiroEncriptado = "C:/Teste/encriptado.txt";
        string ficheiroChave = "C:/Teste/chave.key";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button1.Text = "Abrir";
            button2.Text = "Encriptar";
            button3.Text = "Descodificar";

            //Configuração da caixa de Diálogo
            openFileDialog1.Filter = "Ficheiros de texto (*.txt)|*.txt";
            openFileDialog1.FileName = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK) {

                ficheiroOriginal = openFileDialog1.FileName;
                StreamReader strR = new StreamReader(ficheiroOriginal);
                textBox1.Clear();
                textBox1.Text = strR.ReadToEnd();
                strR.Close();

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FileStream fsFicheiroEncriptado = File.Create(ficheiroEncriptado);
            TripleDESCryptoServiceProvider codificador = new TripleDESCryptoServiceProvider();
            CryptoStream csEncrypt = new CryptoStream(fsFicheiroEncriptado, codificador.CreateEncryptor(), CryptoStreamMode.Write);
            StreamWriter swCodificado = new StreamWriter(csEncrypt);
            StreamReader srFicheiro = new StreamReader(ficheiroOriginal);
            string linha = srFicheiro.ReadLine();
            while (linha != null) {

                swCodificado.Write(linha);
                linha = srFicheiro.ReadLine();

            }
            srFicheiro.Close();
            swCodificado.Flush();
            swCodificado.Close();
            //apresentação do ficheiro codificado
            StreamReader strR = new StreamReader(ficheiroEncriptado);
            textBox2.Clear();
            textBox2.Text = strR.ReadToEnd();
            strR.Close();

            FileStream fsFicheiro = File.Create(ficheiroChave);
            BinaryWriter bwFicheiro = new BinaryWriter(fsFicheiro);
            bwFicheiro.Write(codificador.Key);
            bwFicheiro.Write(codificador.IV);
            bwFicheiro.Flush();
            bwFicheiro.Close();

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            FileStream fsFicheiroEncriptado = File.OpenRead(ficheiroEncriptado);
            FileStream fsFicheiroChave = File.OpenRead(ficheiroChave);
            TripleDESCryptoServiceProvider codificador = new TripleDESCryptoServiceProvider();
            BinaryReader brFicheiro = new BinaryReader(fsFicheiroChave);
            codificador.Key = brFicheiro.ReadBytes(24);
            codificador.IV = brFicheiro.ReadBytes(8);
            CryptoStream csDescodificador = new CryptoStream(fsFicheiroEncriptado, codificador.CreateDecryptor(), CryptoStreamMode.Read);
            StreamReader srDescodificado = new StreamReader(csDescodificador);
            textBox3.Text = srDescodificado.ReadToEnd();
            srDescodificado.Close();
        }
    }
}
