// Fake game types: only the types and members that the mod source reaches.
using Il2CppInterop.Runtime.InteropTypes;

public class SerializableStringField : Il2CppObjectBase
{
    public string Value { get; set; }
}

public class SerializablePrimitiveField<T> : Il2CppObjectBase
{
    // A test sets it to make the read of the value fail.
    public bool Broken;

    private T value;

    public T Value
    {
        get => Broken ? throw new System.InvalidOperationException("broken field") : value;
        set => this.value = value;
    }
}

public class InventoryItem : Il2CppObjectBase
{
    public SerializableStringField identifier;
    public SerializablePrimitiveField<float> weight;

    // A test sets it to make the read of the tag set fail.
    public bool TagsBroken;

    private Il2CppSystem.Collections.Generic.HashSet<ItemTag> tags = new Il2CppSystem.Collections.Generic.HashSet<ItemTag>();

    public Il2CppSystem.Collections.Generic.HashSet<ItemTag> Tags
    {
        get => TagsBroken ? throw new System.InvalidOperationException("broken tags") : tags;
        set => tags = value;
    }

    public InventoryItem(string identifier, float weight, params ItemTag[] tags)
    {
        this.identifier = new SerializableStringField { Value = identifier };
        this.weight = new SerializablePrimitiveField<float> { Value = weight };
        foreach (var tag in tags) this.tags.Add(tag);
    }
}

public enum ItemTag
{
    LARGE = 11,
    HEAVY = 35,
}

public class InventoryManager : Il2CppObjectBase { }
