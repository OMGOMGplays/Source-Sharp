namespace SourceSharp.SP.Public.Engine;

public enum TraceType
{
    TRACE_EVERYTHING = 0,
    TRACE_WORLD_ONLY,
    TRACE_ENTITIES_ONLY,
    TRACE_EVERYTHING_FILTER_PROPS
}

public interface ITraceFilter
{
    public bool ShouldHitEntity(IHandleEntity entity, int contentsMask);
    public TraceType GetTraceType();
}

public class CTraceFilter : ITraceFilter
{
    public bool ShouldHitEntity(IHandleEntity entity, int contentsMask)
    {
        return true;
    }

    public TraceType GetTraceType()
    {
        return TraceType.TRACE_EVERYTHING;
    }
}

public class CTraceFilterEntitiesOnly : ITraceFilter
{
    public bool ShouldHitEntity(IHandleEntity entity, int contentsMask)
    {
        return true;
    }

    public TraceType GetTraceType()
    {
        return TraceType.TRACE_ENTITIES_ONLY;
    }
}

public class CTraceFilterWorldOnly : ITraceFilter
{
    public bool ShouldHitEntity(IHandleEntity entity, int contentsMask)
    {
        return false;
    }

    public TraceType GetTraceType()
    {
        return TraceType.TRACE_WORLD_ONLY;
    }
}

public class CTraceFilterWorldAndPropsOnly : ITraceFilter
{
    public bool ShouldHitEntity(IHandleEntity entity, int contentsMask)
    {
        return false;
    }

    public TraceType GetTraceType()
    {
        return TraceType.TRACE_EVERYTHING;
    }
}

public class CTraceFilterHitAll : CTraceFilter
{
    public new bool ShouldHitEntity(IHandleEntity serverEntity, int contentsMask)
    {
        return true;
    }
}

public interface IEntityEnumerator
{
    public bool EnumEntity(IHandleEntity handleEntity);
}

