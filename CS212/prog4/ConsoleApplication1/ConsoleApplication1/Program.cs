/*
 * @author: Tammie Thong (tt24)
 * @version: 3 Dec 2016
 * 
 * Program.cs
 * 
 * This file creates an unweighted graph of a relationship tree, on a particular file that a user chooses to load.
 * The user can find descendants, friends, and the shortest path between to nodes.
 * 
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

/*
 * Bingo creates an unweighted graph of a relationship tree, on a particular file that a user chooses to load.
 * The user can find descendants, friends, and the shortest path between to nodes.
 **/
namespace Bingo
{
    class Program
    {
        private static RelationshipGraph rg;

        // Read RelationshipGraph whose filename is passed in as a parameter.
        // Build a RelationshipGraph in RelationshipGraph rg
        private static void ReadRelationshipGraph(string filename)
        {
            rg = new RelationshipGraph();                           // create a new RelationshipGraph object

            string name = "";                                       // name of person currently being read
            int numPeople = 0;
            string[] values;
            Console.Write("Reading file " + filename + "\n");
            try
            {
                string input = System.IO.File.ReadAllText(filename);// read file
                input = input.Replace("\r", ";");                   // get rid of nasty carriage returns 
                input = input.Replace("\n", ";");                   // get rid of nasty new lines
                string[] inputItems = Regex.Split(input, @";\s*");  // parse out the relationships (separated by ;)
                foreach (string item in inputItems)
                {
                    if (item.Length > 2)                            // don't bother with empty relationships
                    {
                        values = Regex.Split(item, @"\s*:\s*");     // parse out relationship:name
                        if (values[0] == "name")                    // name:[personname] indicates start of new person
                        {
                            name = values[1];                       // remember name for future relationships
                            rg.AddNode(name);                       // create the node
                            numPeople++;
                        }
                        else
                        {
                            rg.AddEdge(name, values[1], values[0]); // add relationship (name1, name2, relationship)

                            // handle symmetric relationships -- add the other way
                            if (values[0] == "hasSpouse" || values[0] == "hasFriend")
                                rg.AddEdge(values[1], name, values[0]);

                            // for parent relationships add child as well
                            else if (values[0] == "hasParent")
                                rg.AddEdge(values[1], name, "hasChild");
                            else if (values[0] == "hasChild")
                                rg.AddEdge(values[1], name, "hasParent");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write("Unable to read file {0}: {1}\n", filename, e.ToString());
            }
            Console.WriteLine(numPeople + " people read");
        }

        // Show the relationships a person is involved in
        private static void ShowPerson(string name)
        {
            GraphNode n = rg.GetNode(name);
            if (n != null)
                Console.Write(n.ToString());
            else
                Console.WriteLine("{0} not found", name);
        }

        // Show a person's friends
        private static void ShowFriends(string name)
        {
            GraphNode n = rg.GetNode(name);
            if (n != null)
            {
                Console.Write("{0}'s friends: ", name);
                List<GraphEdge> friendEdges = n.GetEdges("hasFriend");
                foreach (GraphEdge e in friendEdges)
                {
                    Console.Write("{0} ", e.To());
                }
                Console.WriteLine();
            }
            else
                Console.WriteLine("{0} not found", name);
        }
        //Show orphans
        private static void ShowOrphans()
        {
            List<GraphNode> getNodes = rg.nodes;
            foreach(GraphNode person in getNodes)
            {
                if(person != null)
                {
                    List<GraphEdge> parents = person.GetEdges("hasParent");
                    int numParents = 0;
                    foreach (GraphEdge parent in parents)
                    {
                        numParents += 1;
                    }
                    if(numParents == 0)
                    {
                        Console.WriteLine(person.Name);
                    }
                }
            }
        }

        /*
         * @param: string name1, string name2
         * Bingo returns the relationship between to people, using a simple BFS
         **/
        private static void Bingo(string name1, string name2)
        {
            Queue<Tuple<GraphNode, string>> nodes = new Queue<Tuple<GraphNode, string>>();
            nodes.Enqueue(new Tuple<GraphNode, string>(rg.GetNode(name1), name1)); //enqueue nodes
            while(nodes.Count > 0)
            {
                Tuple<GraphNode, string> currentNode = nodes.Dequeue();
                currentNode.Item1.toggleVisit(); 

                if(currentNode.Item1.Name == name2) //checks if node you're looking for is the starting one
                {
                    Console.Write(currentNode.Item2); //write this
                    return;
                }
                foreach (GraphEdge relative in currentNode.Item1.GetEdges())
                {
                    if (rg.GetNode(relative.To()).visit ) continue;
                    nodes.Enqueue(new Tuple<GraphNode, string>(rg.GetNode(relative.To()), currentNode.Item2 + " " + relative.Label + " " + relative.To() + "\n"));
                }
            }
        }

        /**
         * @param: string, name
         * Descendants returns children of [name], and calls GrandChildren() on each child
         **/
        private static void Descendants(string name)
        {
            GraphNode n = rg.GetNode(name);
            if (n != null)
            {
                int children = 0;
                List<GraphEdge> childEdges = n.GetEdges("hasChild");
                foreach (GraphEdge e in childEdges) // for each child, print its name
                {
                    Console.WriteLine("child: {0} ", e.To()); 
                    GrandChildren( e.To());
                    children++;
                }
                if (children == 0) // no children
                {
                    Console.WriteLine("{0} has no descendants", name);
                }
                Console.WriteLine();
            }
            else
                Console.WriteLine("{0} has no descendants", name);
        }
        /**
         * @param: string, name
         * Descendants returns children of [name], and calls GreatGrandDescendants() on each grandchild
         **/
        public static void GrandChildren(string name)
        {
            GraphNode n = rg.GetNode(name);
            if (n != null)
            {
                List<GraphEdge> childEdges = n.GetEdges("hasChild");
                foreach (GraphEdge e in childEdges)
                {
                    Console.WriteLine("grandchild: {0} ", e.To()); //for each grandchild, print its name
                    GreatGrandDescendants(e.To(), 1); //find grandchild's descendants
                }
            }
        }
        /**
         * @param: string, name; int, level
         * takes in the name of a particular descendant, and the number of "greats"
         * its descendants are. The method calls itself for each subsequend level of descendant
         * */
        public static void GreatGrandDescendants(string name, int level)
        {
            GraphNode n = rg.GetNode(name);
            if (n != null)
            {
                List<GraphEdge> childEdges = n.GetEdges("hasChild");
                foreach (GraphEdge e in childEdges)
                {
                    for (int i = level; i> 0; i--) //prints the respective number of "greats"
                    {
                        Console.Write("great-");
                    }
                    Console.WriteLine("grandchild: {0} ", e.To());
                    GreatGrandDescendants(e.To(), level + 1); // for the desendant found, call the function on it's descendants
                }
            }
        }
    
        // accept, parse, and execute user commands
        private static void CommandLoop()
        {
            string command = "";
            string[] commandWords;
            Console.Write("Welcome to Harry's Dutch Bingo Parlor!\n");

            while (command != "exit")
            {
                Console.Write("\nEnter a command: ");
                command = Console.ReadLine();
                commandWords = Regex.Split(command, @"\s+");        // split input into array of words
                command = commandWords[0];

                if (command == "exit")
                    ;                                               // do nothing

                // read a relationship graph from a file
                else if (command == "read" && commandWords.Length > 1)
                    ReadRelationshipGraph(commandWords[1]);

                // show information for one person
                else if (command == "show" && commandWords.Length > 1)
                    ShowPerson(commandWords[1]);

                else if (command == "friends" && commandWords.Length > 1)
                    ShowFriends(commandWords[1]);

                else if (command == "orphans")
                    ShowOrphans();

                else if (command == "bingo" && commandWords.Length > 2)
                    Bingo(commandWords[1], commandWords[2]);
                // dump command prints out the graph

                else if (command == "descendants" && commandWords.Length > 1)
                    Descendants(commandWords[1]);

                else if (command == "dump")
                    rg.Dump();

                // illegal command
                else
                    Console.Write("\nLegal commands: read [filename], dump, show [personname],\n  friends [personname], orphans, descendants [personname], exit\n");
            }
        }

        static void Main(string[] args)
        {
            CommandLoop();
        }
    }
}
