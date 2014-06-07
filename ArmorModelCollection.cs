﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

[Serializable]
public class ArmorModelCollection : IEnumerable, IEnumerable<ArmorModel>, IEnumerable<KeyValuePair<ArmorModelSlot, ArmorModel>>
{
    public ArmorModelFeet feet;
    public ArmorModelHead head;
    public ArmorModelLegs legs;
    public ArmorModelTorso torso;

    public ArmorModelCollection()
    {
    }

    public ArmorModelCollection(ArmorModelMemberMap map) : this()
    {
        for (ArmorModelSlot slot = ArmorModelSlot.Feet; slot < ((ArmorModelSlot) 4); slot = (ArmorModelSlot) (((int) slot) + 1))
        {
            this[slot] = map[slot];
        }
    }

    public ArmorModelCollection(ArmorModelMemberMap<ArmorModel> map)
    {
        for (ArmorModelSlot slot = ArmorModelSlot.Feet; slot < ((ArmorModelSlot) 4); slot = (ArmorModelSlot) (((int) slot) + 1))
        {
            this[slot] = map[slot];
        }
    }

    public void CopyFrom(ArmorModel[] array, int offset)
    {
        for (int i = 0; i < 4; i++)
        {
            this[(ArmorModelSlot) ((byte) i)] = array[offset++];
        }
    }

    public int CopyTo(ArmorModel[] array, int offset, int maxCount)
    {
        int num = (maxCount >= 4) ? 4 : maxCount;
        for (int i = 0; i < 4; i++)
        {
            array[offset++] = this[(ArmorModelSlot) ((byte) i)];
        }
        return offset;
    }

    public T GetArmorModel<T>() where T: ArmorModel, new()
    {
        return (T) this[ArmorModelSlotUtility.GetArmorModelSlotForClass<T>()];
    }

    public ArmorModel GetArmorModel(ArmorModelSlot slot)
    {
        return this[slot];
    }

    public Enumerator GetEnumerator()
    {
        return new Enumerator(this);
    }

    public void SetArmorModel<T>(T armorModel) where T: ArmorModel, new()
    {
        this[ArmorModelSlotUtility.GetArmorModelSlotForClass<T>()] = armorModel;
    }

    IEnumerator<ArmorModel> IEnumerable<ArmorModel>.GetEnumerator()
    {
        return new Enumerator(this);
    }

    IEnumerator<KeyValuePair<ArmorModelSlot, ArmorModel>> IEnumerable<KeyValuePair<ArmorModelSlot, ArmorModel>>.GetEnumerator()
    {
        return new Enumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return new Enumerator(this);
    }

    public ArmorModelMemberMap ToMemberMap()
    {
        ArmorModelMemberMap map = new ArmorModelMemberMap();
        for (ArmorModelSlot slot = ArmorModelSlot.Feet; slot < ((ArmorModelSlot) 4); slot = (ArmorModelSlot) (((int) slot) + 1))
        {
            map[slot] = this[slot];
        }
        return map;
    }

    public ArmorModel this[ArmorModelSlot slot]
    {
        get
        {
            switch (slot)
            {
                case ArmorModelSlot.Feet:
                    return this.feet;

                case ArmorModelSlot.Legs:
                    return this.legs;

                case ArmorModelSlot.Torso:
                    return this.torso;

                case ArmorModelSlot.Head:
                    return this.head;
            }
            return null;
        }
        set
        {
            switch (slot)
            {
                case ArmorModelSlot.Feet:
                    this.feet = (ArmorModelFeet) value;
                    break;

                case ArmorModelSlot.Legs:
                    this.legs = (ArmorModelLegs) value;
                    break;

                case ArmorModelSlot.Torso:
                    this.torso = (ArmorModelTorso) value;
                    break;

                case ArmorModelSlot.Head:
                    this.head = (ArmorModelHead) value;
                    break;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Enumerator : IDisposable, IEnumerator, IEnumerator<ArmorModel>, IEnumerator<KeyValuePair<ArmorModelSlot, ArmorModel>>
    {
        private ArmorModelCollection collection;
        private int index;
        internal Enumerator(ArmorModelCollection collection)
        {
            this.collection = collection;
            this.index = -1;
        }

        KeyValuePair<ArmorModelSlot, ArmorModel> IEnumerator<KeyValuePair<ArmorModelSlot, ArmorModel>>.Current
        {
            get
            {
                if ((this.index <= 0) || (this.index >= 4))
                {
                    throw new InvalidOperationException();
                }
                return new KeyValuePair<ArmorModelSlot, ArmorModel>((ArmorModelSlot) ((byte) this.index), this.collection[(ArmorModelSlot) ((byte) this.index)]);
            }
        }
        object IEnumerator.Current
        {
            get
            {
                return this.Current;
            }
        }
        public ArmorModel Current
        {
            get
            {
                return (((this.index <= 0) || (this.index >= 4)) ? null : this.collection[(ArmorModelSlot) ((byte) this.index)]);
            }
        }
        public bool MoveNext()
        {
            return (++this.index < 4);
        }

        public void Reset()
        {
            this.index = -1;
        }

        public void Dispose()
        {
            this = new ArmorModelCollection.Enumerator();
        }
    }
}

