using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using Raven.Client.Documents;
using static System.Formats.Asn1.AsnWriter;

namespace EncounterTracker.Data
{
    public class Registrar : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => DocumentStoreHolder.Store).As<IDocumentStore>().SingleInstance();
        }
    }
}
