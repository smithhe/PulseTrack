using System;
using MediatR;
using PulseTrack.Shared.Responses;

namespace PulseTrack.Application.WorkItems.Commands.DeleteWorkItem;

public sealed record DeleteWorkItemCommand(Guid WorkItemId) : IRequest<ResponseBase>;

