﻿using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using OpenRiaServices.Hosting.Wcf;
using OpenRiaServices.Hosting.Wcf.Behaviors;
using OpenRiaServices.Server;

namespace OpenRiaServices.Hosting
{
    /// <summary>
    /// Represents a SOAP w/ XML encoding endpoint factory for <see cref="DomainService"/>s.
    /// </summary>
    public class BinaryEndpointFactory : DomainServiceEndpointFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SoapXmlEndpointFactory"/> class.
        /// </summary>
        public BinaryEndpointFactory()
        {
        }

        /// <summary>
        /// Creates an endpoint based on the specified address.
        /// </summary>
        /// <param name="contract">The endpoint's contract.</param>
        /// <param name="address">The endpoint's base address.</param>
        /// <returns>An endpoint.</returns>

        protected override ServiceEndpoint CreateEndpointForAddress(ContractDescription contract, Uri address)
        {
            Binding binding = CreateBinding(address);

            ServiceEndpoint endpoint = new ServiceEndpoint(contract, binding, new EndpointAddress(address.OriginalString + "/" + this.Name));
            endpoint.Behaviors.Add(new SoapQueryBehavior());
            return endpoint;
        }

        private static Binding CreateBinding(Uri address)
        {
            var encoding = new BinaryMessageEncodingBindingElement()
            {
            };

            // ServiceUtility.SetReaderQuotas(encoding.ReaderQuotas);


            HttpTransportBindingElement transport;
            if (address.Scheme.Equals(Uri.UriSchemeHttps))
            {
                transport = new HttpsTransportBindingElement();
            }
            else
            {
                transport = new HttpTransportBindingElement();
            }


            /*
            transport.MaxReceivedMessageSize = ServiceUtility.MaxReceivedMessageSize;
            if (ServiceUtility.AuthenticationScheme != AuthenticationSchemes.None)
            {
                transport.AuthenticationScheme = ServiceUtility.AuthenticationScheme;
            }
            */

            return new CustomBinding(encoding, transport);
        }
    }
}
