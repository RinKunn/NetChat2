using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace NetChat2.Api
{
    public class Agent
    {
        IMediator mediator;
        public Agent(IMediator mediator)
        {

        }

        public void ExecuteRequest(IRequest request, CancellationToken token)
        {
            mediator.Send(request, token);
        }
    }


    public class SendTextMessageRequest : IRequest
    {
        public DateTime DateTime { get; private set; }
        public string UserId { get; private set; }
        public string TextMessage { get; private set; }

        public SendTextMessageRequest(string userId, string message, DateTime dateTime)
        {
            UserId = userId;
            TextMessage = message;
            DateTime = dateTime;
        }
    }

    public class SendTextMessageRequestHandler : IRequestHandler<SendTextMessageRequest>
    {

        public Task<Unit> Handle(SendTextMessageRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public class SendLogonMessageRequest : IRequest
    {
        public DateTime DateTime { get; private set; }
        public string UserId { get; private set; }

        public SendLogonMessageRequest(string userId, DateTime dateTime)
        {
            UserId = userId;
            DateTime = dateTime;
        }
    }

    public class SendLogonMessageRequestHandler : IRequestHandler<SendLogonMessageRequest>
    {

        public Task<Unit> Handle(SendLogonMessageRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public class SendLogoutMessageRequest : IRequest
    {
        public DateTime DateTime { get; private set; }
        public string UserId { get; private set; }

        public SendLogoutMessageRequest(string userId, DateTime dateTime)
        {
            UserId = userId;
            DateTime = dateTime;
        }
    }

    public class SendLogoutMessageRequestHandler : IRequestHandler<SendLogoutMessageRequest>
    {

        public Task<Unit> Handle(SendLogoutMessageRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
