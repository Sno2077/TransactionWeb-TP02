Thème personnel : Personnages de OnePiece

Mise en situation:
Les élites mondiaux sont démasqués. Les pirates, les marines et les citoyens s'allient pour rendre l'équilibre dans Le Monde.

Utilisateur visé : Vous êtes un général qui doit gérer des équipes d'expéditions sur l'océan. Marine, Pirates ou Citoyens, tous ont leurs qualités et leurs défauts. Un seul objectif, ramener la paix.

\*\*Pour simplifier, dans le code de ce projet, le mot pirate est utilisé pour définir tout personnage de l'univers de One Piece à l'exception du gouvernement mondial.

== Les comportements attendus: ==

1. Consulter la collection de personnages.
2. Filtrer la collection selon la disponibilité.
3. Consulter un personnage avec un identifiant.
4. Ajouter un personnage au répertoire.
5. Modifier un un personnage existant.
6. Supprimer un un personnage du répertoire.

Propriétés:

- id (int)
- name (string 2-80 char)
- type (List string)
- level (int 1-100)
- bounty (int <=0)
- marine (bool)
- available (bool)

| Méthode et chemin               | Réussite                                                            | Refus principaux                              |
| ------------------------------- | ------------------------------------------------------------------- | --------------------------------------------- |
| `GET /health`                   | `200` et `{ "status": "ok" }`                                       | —                                             |
| `GET /info`                     | `200` et `{ "application": "ServeurPirateTP", "version": "0.1.0" }` | —                                             |
| `GET /api/pirates`              | `200` et collection complète                                        | —                                             |
| `GET /api/pirates?marine=true`  | `200` et collection (marines)                                       | `400` si la valeur n'est ni `true` ni `false` |
| `GET /api/pirates?marine=false` | `200` et collection (pirates)                                       | `400` si la valeur n'est ni `true` ni `false` |
| `GET /api/pirates/{id}`         | `200` et le pirate                                                  | `404`                                         |
| `POST /api/pirates`             | `201`, `Location` et pirate créé                                    | `400`                                         |
| `PUT /api/pirates/{id}`         | `200` et pirate modifié                                             | `400`, `404`                                  |
| `DELETE /api/pirates/{id}`      | `204`                                                               | `404`                                         |

## Corps obligatoires pour `POST` et `PUT`

```json
{
  "name": "string (2-80 caractères, obligatoire)",
  "type": "string (l'un des sept types acceptés, obligatoire)",
  "level": "integer (1-100, obligatoire)",
  "bounty": "integer (>= 0, obligatoire)",
  "marine": "boolean (obligatoire)",
  "available": "boolean (obligatoire)"
}
```

### Causes de refus HTTP 400

- **`name`** : absent, vide, moins de 2 caractères ou plus de 80
- **`type`** : absent ou non dans la liste des sept types (`fighter`, `swordsman`, `navigator`, `doctor`, `Engineer`, `cook`, `sniper`)
- **`level`** : absent, non entier, < 1 ou > 100
- **`bounty`** : absent, non entier ou < 0
- **`marine` ou `available`** : absent ou non booléen
- **JSON malformé** : syntaxe JSON invalide

Données par défaults:

| ID  | Name    | Type      | Level | Bounty | Marine | Available |
| --- | ------- | --------- | ----- | ------ | ------ | --------- |
| 1   | Luffy   | fighter   | 1     | 300000 | false  | true      |
| 2   | Zoro    | swordsman | 1     | 250000 | false  | true      |
| 3   | Garp    | fighter   | 1     | 0      | true   | true      |
| 4   | Bellamy | fighter   | 1     | 195000 | false  | true      |

Structure des données:
json
[
{
"id": 1,
"name": "Luffy",
"type": "fighter",
"level": 1,
"bounty": 300000,
"marine": false,
"available": true
}
]

\*IA utilisée pour formater le tableau dans un .md

Requêtes: Voir fichier .http

Fonctionnalités :

- Lecture
- Filtre (marine true or false)
- Lecture par id
- Creation
- Modification
- Suppression
- Enregistrement des données en JSON

Limites:

- Pas de base de données
- Pas de rôles utilisateurs
- Pas de login
- Pas de redondance des données
