using Envivo.Fresnel.Core;
using Envivo.Fresnel.UiCore.Model;
using Envivo.Fresnel.Utils;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Envivo.Fresnel.UiCore
{
    public class SessionVmBuilder
    {
        private IClock _Clock;
        private Engine _Engine;

        private Lazy<SessionVM> _SessionVM;

        public SessionVmBuilder
            (
            Engine engine,
            IClock clock
            )
        {
            _Engine = engine;
            _Clock = clock;

            _SessionVM = new Lazy<SessionVM>(
                                () => this.CreateSession(),
                                System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);
        }

        public SessionVM Build()
        {
            return _SessionVM.Value;
        }

        private SessionVM CreateSession()
        {
            var result = new SessionVM()
            {
                UserName = Environment.UserName,
                LogonTime = _Clock.Now,
                Messages = this.CreateInfoMessages().ToArray(),
            };
            return result;
        }

        private IEnumerable<MessageVM> CreateInfoMessages()
        {
            var results = new List<MessageVM>();

            results.Add(new MessageVM() { OccurredAt = _Clock.Now, Text = "Welcome to Fresnel" });

            // TODO: Add system & memory info

            results.Add(new MessageVM() { OccurredAt = _Clock.Now, Text = Environment.UserName + " logged on" });

            return results;
        }
    }
}