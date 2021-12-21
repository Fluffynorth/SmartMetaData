namespace SmartMetaData.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class EventTopicAttribute : Attribute
{
    public string Topic { get; set; }

    public EventTopicAttribute(string topic)
    {
        if (string.IsNullOrEmpty(topic))
            throw new ArgumentNullException(nameof(topic));

        Topic = topic;
    }
}
