using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Spectre.Console;

class Program
{
    // Task model
    class Task
    {
        [Key]
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    // In-memory task list
    static List<Task> tasks = new List<Task>();
    static int nextId = 1;

    static void Main(string[] args)
    {
        // Welcome banner
        AnsiConsole.Write(
            new FigletText("Task Manager CLI By AJAL R P")
                .Centered()
                .Color(Color.Green));

        string? command;

        do
        {
            // Main menu prompt
            command = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold cyan]Choose a command[/]:")
                    .AddChoices("Add Task", "View Tasks", "Update Task", "Delete Task", "Exit")
            ).ToLower();

            switch (command)
            {
                case "add task":
                    AddTask();
                    break;
                case "view tasks":
                    ViewTasks();
                    break;
                case "update task":
                    UpdateTask();
                    break;
                case "delete task":
                    DeleteTask();
                    break;
                case "exit":
                    AnsiConsole.MarkupLine("[bold red]Exiting Task Manager. Goodbye![/]");
                    break;
                default:
                    AnsiConsole.MarkupLine("[bold yellow]Invalid command. Try again.[/]");
                    break;
            }
        } while (command != "exit");
    }

    // Add a new task
    static void AddTask()
    {
        var title = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter the [bold green]task title[/]:").PromptStyle("green"));

        var description = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter the [bold green]task description[/]:").PromptStyle("green"));

        var task = new Task
        {
            Id = nextId++,
            Title = title,
            Description = description,
            CreatedAt = DateTime.Now,
            LastUpdated = DateTime.Now
        };

        tasks.Add(task);

        AnsiConsole.MarkupLine($"[bold green]Task ID {task.Id} added successfully![/]");
    }

    // View all tasks
    static void ViewTasks()
    {
        if (tasks.Count == 0)
        {
            AnsiConsole.MarkupLine("[bold yellow]No tasks available.[/]");
            return;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .AddColumn("[bold yellow]ID[/]")
            .AddColumn("[bold yellow]Title[/]")
            .AddColumn("[bold yellow]Description[/]")
            .AddColumn("[bold yellow]Created At[/]")
            .AddColumn("[bold yellow]Last Updated[/]");

        foreach (var task in tasks)
        {
            table.AddRow(
                task.Id.ToString(),
                task.Title,
                task.Description,
                task.CreatedAt.ToString("g"),
                task.LastUpdated.ToString("g")
            );
        }

        AnsiConsole.Write(table);
    }

    // Update a task
    static void UpdateTask()
    {
        var id = AnsiConsole.Prompt(
            new TextPrompt<int>("Enter the [bold cyan]Task ID[/] to update:").PromptStyle("cyan"));

        var task = tasks.Find(t => t.Id == id);
        if (task == null)
        {
            AnsiConsole.MarkupLine("[bold red]Task not found.[/]");
            return;
        }

        var title = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter the new [bold green]task title[/] (leave blank to keep current):")
                .PromptStyle("green")
                .AllowEmpty());

        if (!string.IsNullOrWhiteSpace(title))
        {
            task.Title = title;
        }

        var description = AnsiConsole.Prompt(
            new TextPrompt<string>("Enter the new [bold green]task description[/] (leave blank to keep current):")
                .PromptStyle("green")
                .AllowEmpty());

        if (!string.IsNullOrWhiteSpace(description))
        {
            task.Description = description;
        }

        task.LastUpdated = DateTime.Now;
        AnsiConsole.MarkupLine($"[bold green]Task ID {id} updated successfully![/]");
    }

    // Delete a task
    static void DeleteTask()
    {
        var id = AnsiConsole.Prompt(
            new TextPrompt<int>("Enter the [bold red]Task ID[/] to delete:").PromptStyle("red"));

        var task = tasks.Find(t => t.Id == id);
        if (task == null)
        {
            AnsiConsole.MarkupLine("[bold red]Task not found.[/]");
            return;
        }

        var confirm = AnsiConsole.Confirm($"Are you sure you want to delete Task ID [bold red]{id}[/]?");
        if (confirm)
        {
            tasks.Remove(task);
            AnsiConsole.MarkupLine($"[bold green]Task ID {id} deleted successfully![/]");
        }
        else
        {
            AnsiConsole.MarkupLine("[bold yellow]Task deletion canceled.[/]");
        }
    }
}
