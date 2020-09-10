﻿using System;
using System.IO;
using System.Collections.Generic;

namespace minic
{
	class Program
	{
		static void Main(string[] args)
		{
			//Falta generar EXE
			// y comentarios multilinea

			List<Token> Tokens;

			Console.WriteLine("Arrastre su archivo a la consola");
			string pathfile = Console.ReadLine();
			if (!File.Exists(pathfile))
				Console.WriteLine("El archivo no existe");
			else if (new FileInfo(pathfile).Length == 0)
				Console.WriteLine("El archivo se encuentra vacio");
			else
			{
				// [0] = file name     [1] = file extencion
				string[] fileParts = Path.GetFileName(pathfile).Split('.');
				string pathDirectory = pathfile.Replace(Path.GetFileName(pathfile), "");
				
				AnalizadorLexico ALexico = new AnalizadorLexico(fileParts[0], pathfile, pathDirectory);
				ALexico.Analizar();
				Tokens = ALexico.getTokens();

				AnalizadorSintactico ASintactivo = new AnalizadorSintactico(Tokens);

				foreach (var item in Tokens)
					Console.WriteLine(item.type + "	" + item.content + "	" + item.typeConst);
			}
			Console.ReadKey();
		}
	}
}
