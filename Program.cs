using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

class Program
{
    // Task model
    class Task
    {
        [Key]
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
    }

    // In-memory task list
    static List<Task> tasks = new List<Task>();
    static int nextId = 1;

    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to Task Manager CLI!");
        string? command;

        do
        {
            Console.Write("\nEnter a command (add, view, update, delete, exit): ");
            command = Console.ReadLine()?.Trim().ToLower();

            switch (command)
            {
                case "add":
                    AddTask();
                    break;
                case "view":
                    ViewTasks();
                    break;
                case "update":
                    UpdateTask();
                    break;
                case "delete":
                    DeleteTask();
                    break;
                case "exit":
                    Console.WriteLine("Exiting Task Manager. Goodbye!");
                    break;
                default:
                    Console.WriteLine("Invalid command. Try again.");
                    break;
            }
        } while (command != "exit");
    }

    // Add a new task
    static void AddTask()
    {
        Console.Write("Enter task title: ");
        string? title = Console.ReadLine();

        Console.Write("Enter task description: ");
        string? description = Console.ReadLine();

        var task = new Task
        {
            Id = nextId++,
            Title = title,
            Description = description
        };
        tasks.Add(task);

        Console.WriteLine($"New task added with ID {task.Id}.");
    }

    // View all tasks
    static void ViewTasks()
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("There are no tasks.");
            return;
        }

        const int idWidth = 5;
        const int titleWidth = 20;
        const int descriptionWidth = 30;

        Console.WriteLine(new string('-', idWidth + titleWidth + descriptionWidth + 10)); // Adjust for separators and spaces
        Console.WriteLine($"| {"ID",-idWidth} | {"Title",-titleWidth} | {"Description",-descriptionWidth} |");
        Console.WriteLine(new string('-', idWidth + titleWidth + descriptionWidth + 10));


        foreach (var task in tasks)
        {
            Console.WriteLine($"| {task.Id,-idWidth} | {task.Title,-titleWidth} | {task.Description,-descriptionWidth} |");
        }

        Console.WriteLine(new string('-', idWidth + titleWidth + descriptionWidth + 10));
    }


    // Update a task
    static void UpdateTask()
    {
        Console.Write("Enter task ID to update: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        var task = tasks.Find(t => t.Id == id);
        if (task == null)
        {
            Console.WriteLine("Task not found.");
            return;
        }

        Console.Write("Enter new title (leave blank to keep current): ");
        string? title = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(title))
        {
            task.Title = title;
        }

        Console.Write("Enter new description (leave blank to keep current): ");
        string? description = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(description))
        {
            task.Description = description;
        }

        Console.WriteLine($"Task ID {id} updated.");
    }

    // Delete a task
    static void DeleteTask()
    {
        Console.Write("Enter task ID to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        var task = tasks.Find(t => t.Id == id);
        if (task == null)
        {
            Console.WriteLine("Task not found.");
            return;
        }

        tasks.Remove(task);
        Console.WriteLine($"Task ID {id} deleted.");
    }
}
