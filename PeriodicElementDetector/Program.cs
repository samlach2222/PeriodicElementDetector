﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DDR_GraphMix
{
    class Program
    {
        static readonly Dictionary<string, int> elementList = new();
        static string requestedWord;
        static List<String> WordComposition = new();
        static List<String> dictionnary = new();
        static sbyte lastPercentage = -1;

        static void Main()
        {
            ElementListFilling(); // fill the table


            // SINGLE WORD
            //FindAWord(); // find a single word
            //CreateTableTraitment(requestedWord);
            //string encode = StringTraitment();
            //if (encode != "")
            //{
            //    Console.WriteLine("The word is found");
            //    Console.WriteLine("The code is " + encode);
            //}
            //else
            //{
             //   Console.WriteLine("The word is not found");
            //}

            
            // ALL WORDS IN DICTIONNARY
            DictionnaryFilling();
            Dictionary<String, String> result = FindAllWordInDictionnary();
            CreateTxtFileDictionnary(result);
        }

        /****************************************************************/
        /*                        DICTIONNARY PART                      */
        /****************************************************************/
        
        static void DictionnaryFilling()
        {
            using (StreamReader streamReader = new StreamReader(@"Resources\liste.de.mots.francais.frgut.txt"))
            {
                long fileLength = streamReader.BaseStream.Length;
                while (!streamReader.EndOfStream)
                {
                    ShowProgression(streamReader.BaseStream.Position, fileLength);
                    string readLine = streamReader.ReadLine();
                    // to lower
                    readLine = readLine.ToLower();
                    // remove all accent
                    readLine = readLine.Replace("é", "e");
                    readLine = readLine.Replace("è", "e");
                    readLine = readLine.Replace("ê", "e");
                    readLine = readLine.Replace("à", "a");
                    readLine = readLine.Replace("â", "a");
                    readLine = readLine.Replace("ô", "o");
                    readLine = readLine.Replace("î", "i");
                    readLine = readLine.Replace("ç", "c");
                    readLine = readLine.Replace("ù", "u");
                    readLine = readLine.Replace("û", "u");
                    readLine = readLine.Replace("ï", "i");
                    readLine = readLine.Replace("ü", "u");
                    readLine = readLine.Replace("ÿ", "y");
                    readLine = readLine.Replace("œ", "oe");

                    if (!readLine.Contains('-'))
                    {
                        dictionnary.Add(readLine);
                    }
                }
                Console.WriteLine();
                Console.WriteLine("Dictionnary is filled");
            }
        }

        static Dictionary<String, String> FindAllWordInDictionnary()
        {
            Dictionary<String, String> result = new();
            int pos = 0;
            int total = dictionnary.Count -1;

            foreach (string word in dictionnary)
            {
                ShowProgression(pos, total);
                CreateTableTraitment(word);
                string encode = StringTraitment();
                if (encode != "")
                {
                    result.Add(word, encode);
                }
                pos++;
                Console.WriteLine(pos);
            }
            return result;
        }

        static void CreateTxtFileDictionnary(Dictionary<String, String> list)
        {
            // create a new txt file
            using (StreamWriter streamWriter = new StreamWriter(@"Resources\dictionnaryPeriodicElement.txt"))
            {
                foreach (KeyValuePair<String, String> pair in list)
                {
                    streamWriter.WriteLine(pair.Key + "\t" + pair.Value);
                }
            }
        }

        static void FindAWord()
        {
            Console.Write("Enter your word : ");
            requestedWord = Console.ReadLine();
        }
        
        static void ElementListFilling()
        {
            using (StreamReader streamReader = new("Resources/PeriodicTable.dat"))
            {
                while (!streamReader.EndOfStream)
                {
                    string readLine = streamReader.ReadLine();
                    string[] splitedLine = readLine.Split("\t"); // get the two ints of the line
                    string elementLetter = splitedLine[0];
                    int elementNumber = Int32.Parse(splitedLine[1]);
                    elementList.Add(elementLetter, elementNumber);
                }
            }
        }

        static int SearchLetters(string letters)
        {
            int value = -1;
            foreach (string getLetter in elementList.Keys)
            {
                if (getLetter == letters)
                {
                    value = elementList[getLetter];
                }
            }
            return value;
        }

        static void CreateTableTraitment(string word)
        {
            // Manage single chars
            foreach (char letter in word)
            {
                if (!WordComposition.Contains(letter.ToString()))
                {
                    if (SearchLetters(letter.ToString()) != -1)
                    {
                        WordComposition.Add(letter.ToString());
                    }
                }
            }

            // Manage char duet
            for (int i = 0; i < word.Length - 1; i++)
            {
                string letters = word[i].ToString() + word[i + 1].ToString();
                if (!WordComposition.Contains(letters.ToString()))
                {
                    if (SearchLetters(letters.ToString()) != -1)
                    {
                        WordComposition.Add(letters.ToString());
                    }
                }
            }
        }

        static string StringTraitment()
        {
            string returnEncode = "";

            // get all flush possible for WordComposition
            double count = Math.Pow(2, WordComposition.Count);
            for (int i = 1; i <= count - 1; i++)
            {
                string attempt = "";
                string encode = "";
                for (int j = 0; j < WordComposition.Count; j++)
                {
                    int b = i & (1 << j);
                    if (b > 0)
                    {
                        encode += elementList[WordComposition[j]].ToString() + " ";
                        attempt += WordComposition[j];
                    }
                }
                if (attempt == requestedWord) // if word finded
                {
                    goto end;
                    Console.WriteLine(encode);
                    returnEncode = encode;
                }
            }
            end:
            return returnEncode;
        }

        /// <summary>
        /// Show the progression using a bar progressively filled with asterisks
        /// </summary>
        /// <param name="value">Value to calculate a percentage of progression</param>
        /// <param name="max">Maximum value of value to calculate a percentage of progression</param>
        public static void ShowProgression(long value, long max)
        {
            long pourcentage = value * 100 / max;
            if (pourcentage < lastPercentage)  // A new progression is happening
            {
                lastPercentage = -1;
            }
            if (pourcentage > lastPercentage)
            {
                lastPercentage = (sbyte)pourcentage;
                string barre = "";
                for (int i = 0; i < 10; i++)
                {
                    if (pourcentage / 10 > i)
                    {
                        barre += '*';
                    }
                    else
                    {
                        barre += ' ';
                    }
                }
                Console.Write("\r[" + barre + "] " + pourcentage + '%');
            }
        }
    }
}