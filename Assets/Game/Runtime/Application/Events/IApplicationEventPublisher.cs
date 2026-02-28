namespace Game.Application.Events
{
    public interface IApplicationEventPublisher
    {
        void Publish(ApplicationEvent applicationEvent);
    }
}
