using Taskly.Domain.Todos.Events;
using Taskly.Domain.Todos.ValueObjects;
using Taskly.SharedKernel.Common;

namespace Taskly.Domain.Todos.Entities
{
    public sealed class Todo : AggregateRoot<Guid>
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public Expiry Expiry { get; private set; }
        public Percent PercentComplete { get; private set; }

        private Todo() { }

        private Todo(Guid id, string title, string description, Expiry expiry, Percent percentComplete)
        {   
            Id = id;
            Title = title;
            Description = description;
            Expiry = expiry;
            PercentComplete = percentComplete;
        }

        public static Result<Todo> Create(string title, string description, Expiry expiry)
        {
            if (string.IsNullOrWhiteSpace(title))
                return Result.Failure<Todo>(Error.Failure("Todo.EmptyTitle", "Title cannot be empty."));
        
            if (title.Length > 100)
                return Result.Failure<Todo>(Error.Failure("Todo.TitleTooLong", "Title cannot exceed 100 characters."));

            var todo = new Todo(Guid.NewGuid(), title, description, expiry, Percent.Zero);
            
            todo.AddDomainEvent(new TaskCreatedDomainEvent(todo.Id, todo.Expiry.Value));
            
            return Result.Success(todo);
        }
        
        public Result Update(string title, string description, Expiry expiry)
        {
            if (string.IsNullOrWhiteSpace(title))
                return Result.Failure(Error.Failure("Todo.EmptyTitle", "Title cannot be empty."));
            
            Title = title;
            Description = description;
            Expiry = expiry;

            AddDomainEvent(new UpdateTaskDomainEvent(Id, Expiry.Value));

            return Result.Success();
        }
        
        public Result SetPercentComplete(Percent percentComplete)
        {
            if (percentComplete == Percent.Zero)
                return Result.Failure(Error.Failure("Todo.InvalidTransition", "Cannot explicitly set to 0%. Task is already initialized."));

            if (percentComplete == Percent.Completed)
                return Result.Failure(Error.Failure("Todo.UseDedicatedMethod", "Cannot set to 100% here. Use the explicit MarkAsDone() method!"));
            
            if (PercentComplete == Percent.Completed)
                return Result.Failure(Error.Failure("Todo.Immutable", "Cannot change progress of an already completed task."));

            PercentComplete = percentComplete;
            
            AddDomainEvent(new SetPercentCompleteDomainEvent(Id));
            
            return Result.Success();
        }
        
        public Result MarkAsDone()
        {
            if (PercentComplete == Percent.Zero)
                return Result.Failure(Error.Failure("Todo.NotStarted", "Cannot complete task with 0% progress!"));

            if (PercentComplete == Percent.Completed)
                return Result.Failure(Error.Failure("Todo.AlreadyCompleted", "Task has been already completed!"));
    
            PercentComplete = Percent.Completed;
            
            AddDomainEvent(new SetPercentCompleteDomainEvent(Id));
            
            return Result.Success();
        }
    }
}