using Microsoft.EntityFrameworkCore;
using ToDoApp.Application.Database;
using ToDoApp.Application.Database.Repositories;
using ToDoApp.Application.Dto;
using ToDoApp.Common.Abstractions;

namespace ToDoApp.Application.Features.Queries;

public class GetGroupsQuery : IQuery<List<TaskGroupDto>>
{
    internal sealed class Handler : IQueryHandler<GetGroupsQuery, List<TaskGroupDto>>
    {
        private readonly IToDoDbContext _context;
        private readonly IUserContext _userContext;

        public Handler(IUserContext userContext, IToDoDbContext context)
        {
            _userContext = userContext;
            _context = context;
        }

        public async Task<List<TaskGroupDto>> Handle(GetGroupsQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == _userContext.UserId, cancellationToken);

            if (user is null)
                throw new InvalidOperationException("User not found");

            var groups = await _context.TaskGroups
                .Include(g => g.Tasks)
                .Where(g => g.OwnerId == user.Id)
                .Select(x => TaskGroupDto.AsDto(x))
                .ToListAsync(cancellationToken);

            if (groups is null)
                throw new InvalidOperationException("No groups found for the user");

            return groups;
        }
    }
}