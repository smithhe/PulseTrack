using System;
using MediatR;
using PulseTrack.Shared.Dtos;
using PulseTrack.Shared.Responses;

namespace PulseTrack.Application.WorkItems.Queries.GetWorkItem;

public sealed record GetWorkItemQuery(Guid WorkItemId) : IRequest<Response<WorkItemDetails>>;

