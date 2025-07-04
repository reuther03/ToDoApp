using MediatR;

namespace ToDoApp.Common.Abstractions;

public interface IQuery<out TResponse> : IRequest<TResponse>;