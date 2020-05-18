using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using System;

public class Ability {
    // Originally type was denoted by an enum, this was diffuclt to get working properly with the xml sheet
    // At least for now the enum has been replaced by an int, which brings its own problems but will at least work
    // The current types of strings to be handled (direct comparison) are
    // 0, Damaging (ability that does damage)
    // 1, Healing (ability that heals damage to a friendly target)
    // These are to also be stored in Constants to be tracked easily, and documented in the xml sheet abilities.xml as well
    [XmlAttribute("name")]
    public string name;
    [XmlAttribute("type")]
    public int type;
    [XmlAttribute("cooldown")]
    public float cooldown;
    [XmlAttribute("power")]
    public float power; // Power applies for damage/healing/any other effects (i.e. shielding, buffs)
    [XmlAttribute("range")]
    public float range;
}
