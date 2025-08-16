using System;
using MediatR;

namespace PulseTrack.Application.Features.Labels.Commands;

public record UnassignLabelCommand(Guid ItemId, Guid LabelId) : IRequest<Unit>;
