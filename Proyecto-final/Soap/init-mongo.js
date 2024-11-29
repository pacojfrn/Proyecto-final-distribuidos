db = db.getSiblingDB('test');

db.createCollection('Persona');
db.Persona.insertMany([{
    "arcana": "Devil",
    "weak": ["Ice", "Light"],
    "stats": {
      "St": 56,
      "Ma": 67,
      "En": 52,
      "Ag": 34,
      "Lu": 47
    },
    "strength": ["Slash", "Strike", "Fire", "Darkness"],
    "level": 81,
    "name": "Abaddon"
  },
  {
    "arcana": "Death",
    "weak": ["Light"],
    "stats": {
      "St": 38,
      "Ma": 67,
      "En": 35,
      "Ag": 52,
      "Lu": 46
    },
    "strength": ["Darkness"],
    "level": 73,
    "name": "Alice"
  },
  {
    "arcana": "Empress",
    "weak": ["Darkness"],
    "stats": {
      "St": 51,
      "Ma": 73,
      "En": 64,
      "Ag": 53,
      "Lu": 57
    },
    "strength": ["Slash", "Strike", "Fire", "Ice", "Wind"],
    "level": 89,
    "name": "Alilat"
  },
  {
    "arcana": "Aeon",
    "weak": ["Slash", "Electricity"],
    "stats": {
      "St": 55,
      "Ma": 55,
      "En": 71,
      "Ag": 39,
      "Lu": 42
    },
    "strength": ["Pierce", "Ice", "Light"],
    "level": 78,
    "name": "Ananta"
  },
  {
    "arcana": "Justice",
    "weak": ["Darkness"],
    "stats": {
      "St": 4,
      "Ma": 5,
      "En": 3,
      "Ag": 4,
      "Lu": 3
    },
    "strength": ["Electricity", "Light"],
    "level": 4,
    "name": "Angel"
  },
  {
    "arcana": "Judgment",
    "weak": [],
    "stats": {
      "St": 30,
      "Ma": 42,
      "En": 33,
      "Ag": 22,
      "Lu": 30
    },
    "strength": ["Light", "Darkness"],
    "level": 46,
    "name": "Anubis"
  },
  {
    "arcana": "Priestess",
    "weak": ["Fire"],
    "stats": {
      "St": 2,
      "Ma": 4,
      "En": 2,
      "Ag": 3,
      "Lu": 2
    },
    "strength": ["Ice"],
    "level": 2,
    "name": "Apsaras"
  },
  {
    "arcana": "Chariot",
    "weak": ["Wind"],
    "stats": {
      "St": 8,
      "Ma": 3,
      "En": 4,
      "Ag": 5,
      "Lu": 5
    },
    "strength": ["Strike", "Fire"],
    "level": 6,
    "name": "Ara Mitama"
  },
  {
    "arcana": "Hermit",
    "weak": ["Strike", "Ice"],
    "stats": {
      "St": 52,
      "Ma": 55,
      "En": 53,
      "Ag": 38,
      "Lu": 37
    },
    "strength": ["Slash", "Strike", "Light", "Darkness"],
    "level": 73,
    "name": "Arahabaki"
  },
  {
    "arcana": "Justice",
    "weak": ["Electricity", "Darkness"],
    "stats": {
      "St": 10,
      "Ma": 9,
      "En": 9,
      "Ag": 10,
      "Lu": 8
    },
    "strength": ["Slash", "Light"],
    "level": 13,
    "name": "Arcangel"
  },
  {
    "arcana": "Sun",
    "weak": ["Wind"],
    "stats": {
      "St": 81,
      "Ma": 57,
      "En": 52,
      "Ag": 51,
      "Lu": 59
    },
    "strength": ["Strike", "Fire", "Light"],
    "level": 91,
    "name": "Asura"
  },
  {
    "arcana": "Strength",
    "weak": ["Ice"],
    "stats": {
      "St": 67,
      "Ma": 40,
      "En": 67,
      "Ag": 41,
      "Lu": 38
    },
    "strength": ["Pierce", "Light"],
    "level": 77,
    "name": "Atavaka"
  },
  {
    "arcana": "Fortune",
    "weak": ["Fire"],
    "stats": {
      "St": 26,
      "Ma": 50,
      "En": 35,
      "Ag": 39,
      "Lu": 47
    },
    "strength": ["Wind", "Light", "Darkness"],
    "level": 62,
    "name": "Atropos"
  },
  {
    "arcana": "Hanged",
    "weak": ["Light", "Darkness"],
    "stats": {
      "St": 47,
      "Ma": 57,
      "En": 55,
      "Ag": 62,
      "Lu": 32
    },
    "strength": ["Slash", "Strike", "Pierce", "Fire", "Wind"],
    "level": 78,
    "name": "Attis"
  },
  {
    "arcana": "Moon",
    "weak": ["Fire", "Light"],
    "stats": {
      "St": 57,
      "Ma": 57,
      "En": 47,
      "Ag": 45,
      "Lu": 44
    },
    "strength": ["Ice", "Electricity", "Wind", "Darkness"],
    "level": 77,
    "name": "Baal Zebul"
  },
  {
    "arcana": "Devil",
    "weak": ["Wind", "Light"],
    "stats": {
      "St": 20,
      "Ma": 25,
      "En": 22,
      "Ag": 16,
      "Lu": 19
    },
    "strength": ["Fire", "Darkness"],
    "level": 30,
    "name": "Baphomet"
  },
  {
    "arcana": "Empreror",
    "weak": ["Wind"],
    "stats": {
      "St": 45,
      "Ma": 57,
      "En": 46,
      "Ag": 36,
      "Lu": 33
    },
    "strength": ["Electricity", "Light"],
    "level": 68,
    "name": "Barong"
  },
  {
    "arcana": "Devil",
    "weak": ["Fire", "Light"],
    "stats": {
      "St": 52,
      "Ma": 67,
      "En": 54,
      "Ag": 54,
      "Lu": 52
    },
    "strength": ["Strike", "Electricity", "Wind", "Darkness"],
    "level": 86,
    "name": "Belcebu"
  },
  {
    "arcana": "Emperor",
    "weak": ["Electricity", "Light"],
    "stats": {
      "St": 36,
      "Ma": 47,
      "En": 41,
      "Ag": 30,
      "Lu": 33
    },
    "strength": ["Ice", "Electricity", "Darkness"],
    "level": 58,
    "name": "Belphegor"
  },
  {
    "arcana": "Hierophant",
    "weak": ["Electricity"],
    "stats": {
      "St": 12,
      "Ma": 7,
      "En": 10,
      "Ag": 11,
      "Lu": 6
    },
    "strength": ["Fire", "Wind"],
    "level": 13,
    "name": "Berith"
  },
  {
    "arcana": "Tower",
    "weak": ["Ice"],
    "stats": {
      "St": 50,
      "Ma": 38,
      "En": 45,
      "Ag": 36,
      "Lu": 30
    },
    "strength": ["Strike", "Fire", "Light"],
    "level": 60,
    "name": "Bishamonten"
  },
  {
    "arcana": "Fool",
    "weak": ["Light"],
    "stats": {
      "St": 26,
      "Ma": 37,
      "En": 25,
      "Ag": 30,
      "Lu": 20
    },
    "strength": ["Fire", "Ice", "Darkness"],
    "level": 42,
    "name": "Escarcha Negra"
  },
  {
    "arcana": "Temperance",
    "weak": ["Fire"],
    "stats": {
      "St": 50,
      "Ma": 48,
      "En": 51,
      "Ag": 42,
      "Lu": 35
    },
    "strength": ["Ice", "Electricity"],
    "level": 69,
    "name": "Byakko"
  },
  {
    "arcana": "Moon",
    "weak": ["Fire"],
    "stats": {
      "St": 48,
      "Ma": 41,
      "En": 31,
      "Ag": 35,
      "Lu": 32
    },
    "strength": ["Slash", "Pierce", "Darkness"],
    "level": 56,
    "name": "Chernobog"
  },
  {
    "arcana": "Tower",
    "weak": ["Electricity"],
    "stats": {
      "St": 73,
      "Ma": 56,
      "En": 53,
      "Ag": 55,
      "Lu": 63
    },
    "strength": ["Slash", "Pierce"],
    "level": 91,
    "name": "Chi tu"
  },
  {
    "arcana": "Chariot",
    "weak": ["Darkness"],
    "stats": {
      "St": 11,
      "Ma": 6,
      "En": 19,
      "Ag": 4,
      "Lu": 9
    },
    "strength": ["Strike", "Fire"],
    "level": 14,
    "name": "Quimera"
  },
  {
    "arcana": "Fortune",
    "weak": ["Fire"],
    "stats": {
      "St": 20,
      "Ma": 29,
      "En": 22,
      "Ag": 24,
      "Lu": 25
    },
    "strength": ["Wind"],
    "level": 37,
    "name": "Clotho"
  },
  {
    "arcana": "Tower",
    "weak": ["Ice"],
    "stats": {
      "St": 40,
      "Ma": 33,
      "En": 25,
      "Ag": 30,
      "Lu": 23
    },
    "strength": ["Pierce", "Electricity", "Wind"],
    "level": 46,
    "name": "Cu chulainn"
  },
  {
    "arcana": "Lovers",
    "weak": ["Electricity"],
    "stats": {
      "St": 40,
      "Ma": 59,
      "En": 40,
      "Ag": 47,
      "Lu": 49
    },
    "strength": ["Fire", "Ice", "Light"],
    "level": 72,
    "name": "Cibeles"
  },
  {
    "arcana": "Hierophant",
    "weak": ["Darkness"],
    "stats": {
      "St": 37,
      "Ma": 52,
      "En": 40,
      "Ag": 33,
      "Lu": 46
    },
    "strength": ["Light"],
    "level": 64,
    "name": "Daisoujou"
  },
  {
    "arcana": "Hermit",
    "weak": ["Ice"],
    "stats": {
      "St": 36,
      "Ma": 22,
      "En": 16,
      "Ag": 29,
      "Lu": 18
    },
    "strength": ["Strike", "Fire"],
    "level": 38,
    "name": "Dakini"
  },
  {
    "arcana": "Fool",
    "weak": ["Slash"],
    "stats": {
      "St": 41,
      "Ma": 37,
      "En": 36,
      "Ag": 38,
      "Lu": 38
    },
    "strength": [],
    "level": 59,
    "name": "Decarabia"
  }
]);