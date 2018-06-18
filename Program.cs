﻿using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Runtime;
using TagLib;
using TagLib.Id3v2;


namespace AddToMp3
{
    class Program
    {


        static void Main(string[] args)
        {
            Encoding FileEncoding = Encoding.ASCII;//Define the encoding.
            TagLib.File Mp3File = null;// = TagLib.File.Create(Mp3Path); //Create TagLib file... ensures a Id3v2 header.;
            TagLib.Ape.Tag ApeTag = null;// = Methods.CheckApeTag(Mp3File); //return existing tag. create one if none.
            string[] collectCloudCoinStack = new string[3];
            string[] endState = new string[6]; //Keeps the current state of each case.
            string Mp3Path = null;// = Methods.ReturnMp3FilePath(); //Save file path.
            int userChoice = Methods.printOptions() + 1;
            bool menuStyle = true; // Discriptive / Standard
            bool makingChanges = true; //Keeps the session runnning.



            Methods.printWelcome();
            while (makingChanges){
                switch(userChoice){
                    case 1: //Select .mp3 file.
                        selectMp3();
                    break;
                    case 2://Select .stack file from Bank folder
                        selectStack();
                    break;
                    case 3://Insert the .stack file into the .mp3 file 
                        stackToMp3();
                    break;
                    case 4://Return .stack from .mp3
                        stackFromMp3();
                    break;
                    case 5://Delete .stack from .mp3 
                        deleteFromMp3();
                    break;
                    case 6://Save .mp3's current state
                        saveMp3();
                    break;
                    case 7://Quit
                        makingChanges = false;
                    break;
                    case 8://Descriptions
                        menuStyle = !menuStyle;
                    break;
                    default:
                        Console.Out.WriteLine("No matches for input!");
                    break;
                }

                ///Switch bestween discriptive and standard menus options.
                switch(menuStyle){
                    case true:
                        Console.Clear();
                        Methods.printStates(endState);
                        userChoice = Methods.printOptions() + 1;
                    break;
                    case false:
                        Console.Clear();
                        Methods.printStates(endState);
                        userChoice = Methods.printHelp() + 1;  
                    break;
                }//end switch
            }//end while loop.
            Console.Out.WriteLine("Goodbye");

        void selectMp3()
        {
            Mp3Path = Methods.ReturnMp3FilePath(); //Save file path.
            Mp3File = TagLib.File.Create(Mp3Path); //Create TagLib file... ensures a Id3v2 header. 
            ApeTag = Methods.CheckApeTag(Mp3File); //return existing tag. create one if none.
            string fileName = Mp3File.Name;
            endState[0] = "MP3 file: " + fileName + " has been selected. ";
        }// end selectMp3
        void selectStack()
        {
                try
                {
                collectCloudCoinStack = Methods.collectBankStacks(); //Select stacks to insert. 
                endState[1] = "Stack: " + collectCloudCoinStack[0];
                }//end try
                catch
                {
                endState[1] = ".stack error ";
                }//end catch
                Console.Out.WriteLine(endState[1]);
        }// end selectStack
        void stackToMp3()
        {
                string cloudCoinStack = collectCloudCoinStack[1];
                string stackName = collectCloudCoinStack[2];
                Console.Out.WriteLine("Existing Stacks in the mp3 will be overwritten");
                Console.Out.WriteLine("Enter/Return to continue, Any other key to go back.");
                if(Console.ReadKey(true).Key == ConsoleKey.Enter)//prompt user to continue.
                {
                    if(cloudCoinStack != null && ApeTag != null)
                    {
                        Console.Out.WriteLine("Existing Stacks in the mp3 will be overwritten");
                        ApeTag = Methods.CheckApeTag(Mp3File);
                        Methods.SetApeTagValue(ApeTag, cloudCoinStack, stackName);
                        endState[2] = ".stack was successfully inserted in " + Mp3File.Name;
                        endState[4] = "Stacks in " + Mp3File.Name + " have been added.";
                    }//end if
                    else
                    {
                        Methods.SetApeTagValue(ApeTag, cloudCoinStack, stackName);
                        endState[2] = "No saved cloud coin stack.";
                    }//end else
                    Console.Out.WriteLine(endState[2]);
                }//end if
        }//end stackToMp3
        void stackFromMp3()
        {
                Mp3File = TagLib.File.Create(Mp3Path);
                if(Mp3File != null)
                {
                    string mp3CurrentCoinStack = Methods.ReturnCloudCoinStack(Mp3File);//The current stack from the mp3 gets saved.
                    if(mp3CurrentCoinStack != "null")
                    {
                        endState[3] = "A file was created:  " + mp3CurrentCoinStack;
                    }//end if.
                    else{
                        endState[3] = "Incorrect key press.";
                    }//end else.
                    Console.Out.WriteLine(endState[3]);
                }//end if.
                else
                {
                    Console.Out.WriteLine("No mp3 file selected.");
                }//end else.
        } //end stackFromMp3
        void deleteFromMp3(){
                Console.Out.WriteLine("WARNING: you are about to permenantley delete any stack files found in " + Mp3File.Name);
                Console.Out.WriteLine("Enter/Return to continue, Any other key to go back.");
                if(Console.ReadKey(true).Key == ConsoleKey.Enter)
                {
                Methods.RemoveExistingStacks(ApeTag);
                endState[4] = "Any existing stacks in " + Mp3File.Name + " have been deleted.";
                }//end if.
                else
                {
                endState[4] = "Stacks in " + Mp3File.Name + " have NOT been deleted.";
                }//end else.
                Console.Out.WriteLine(endState[4]);
        } // end deleteFromMp3.
        void saveMp3()
        {
                Mp3File.Save(); // Save changes.
                endState[5] = Mp3File.Name + " has been saved with the changes made";
                Console.Out.WriteLine(endState[5]);
        }//end saveMp3

        }//end main.
            

    }//end Program
}//end addToMp3

//Removed code
            // TagLib.Id3v2.Tag Mp3Tag = (TagLib.Id3v2.Tag)Mp3File.GetTag(TagTypes.Id3v2);
            // Methods.CreateAnId3Frame(Mp3File, Mp3Tag, MyCloudCoin, FileEncoding); // Create private frame.
            // Methods.ReadAFrame(Mp3File, Mp3Tag, FileEncoding); // Read contents of private frame.
