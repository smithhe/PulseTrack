using System;
using MediatR;
using PulseTrack.Domain.Entities;

namespace PulseTrack.Application.Features.Items.Commands
{
    public record CreateItemCommand(Guid ProjectId, Guid? SectionId, string Content, string? DescriptionMd, int Priority, bool Pinned) : IRequest<Item>;
}
