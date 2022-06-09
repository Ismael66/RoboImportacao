using System;
using Microsoft.Xrm.Sdk;

namespace Plugin
{
    public abstract partial class Base : IPlugin
    {
        public abstract void Execute();
        protected IPluginExecutionContext Context { get; set; }
        protected IOrganizationServiceFactory ServiceFactory { get; set; }
        protected IOrganizationService AdminService { get; set; }
        protected IOrganizationService UserService { get; set; }
        protected ITracingService Trace { get; set; }
        public void Execute(IServiceProvider serviceProvider)
        {
            this.Context = serviceProvider.GetService(typeof(IPluginExecutionContext)) as IPluginExecutionContext;
            this.ServiceFactory = serviceProvider.GetService(typeof(IOrganizationServiceFactory)) as IOrganizationServiceFactory;
            this.UserService = this.ServiceFactory.CreateOrganizationService(this.Context.UserId);
            this.AdminService = this.ServiceFactory.CreateOrganizationService(null);
            this.Trace = serviceProvider.GetService(typeof(ITracingService)) as ITracingService;
            this.Execute();
        }
        protected T GetInputParameter<T>(string keyName = "Target")
        {
            if (this.Context?.InputParameters.ContainsKey(keyName) is true
            && this.Context.InputParameters[keyName] is T input)
            {
                return input;
            }
            return default;
        }
        protected Entity GetPreImage(string keyName = "preImage")
        {
            if (this.Context?.PreEntityImages.ContainsKey(keyName) is true
            && this.Context.PreEntityImages[keyName] is Entity preImage)
            {
                return preImage;
            }
            return default;
        }
        protected Entity GetPostImage(string keyName = "postImage")
        {
            if (this.Context?.PostEntityImages.ContainsKey(keyName) is true
            && this.Context.PostEntityImages[keyName] is Entity postImage)
            {
                return postImage;
            }
            return default;
        }
        protected bool Validate(MessageName message, Stage stage, Mode mode)
        {
            return this.Context?.MessageName.ToLower() == Enum.GetName(typeof(MessageName),message).ToLower()
                && this.Context.Mode == (int)mode
                && this.Context.Stage == (int)stage;
        }
    }
}