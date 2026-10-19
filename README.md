## Prérequis

- **.NET 10** ou supérieur
## Démarrage

1. Ouvre un terminal dans le dossier racine du projet `TransactionWeb-TP02`
2. Exécute :
```bash
   dotnet run
```
3. L'API démarre sur `http://localhost:5050`

L'application charge les données par défaut depuis `pirates.json` au premier lancement.

## Thème personnel : Personnages de OnePiece

**Mise en situation** : les élites mondiales sont démasquées. Les pirates, les marines et les citoyens s'allient pour rendre l'équilibre au monde.

**Utilisateur visé** : un général qui gère des équipes d'expéditions sur l'océan. Marine, pirates ou citoyens, tous ont leurs qualités et leurs défauts. Un seul objectif : ramener la paix.

*Pour simplifier, dans le code, le mot `pirate` désigne tout personnage de l'univers de One Piece, sauf le gouvernement mondial.

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
- type (List string) "`fighter`, `swordsman`, `navigator`, `doctor`, `ingeneer`, `cook`, sniper`"
- level (int 1-100)
- bounty (int >=0)
- marine (bool)
- available (bool)

| Méthode et chemin                                             | Réussite                                                            | Refus principaux                              |
| ------------------------------------------------------------- | ------------------------------------------------------------------- | --------------------------------------------- |
| `GET /health`                                                 | `200` et `{ "status": "ok" }`                                       | —                                             |
| `GET /info`                                                   | `200` et `{ "application": "ServeurPirateTP", "version": "0.1.0" }` | —                                             |
| `GET /api/pirates`                                            | `200` et collection complète                                        | —                                             |
| `GET /api/pirates?marine=true`                                | `200` et collection (marines)                                       | `400` si la valeur n'est ni `true` ni `false` |
| `GET /api/pirates?marine=false`                               | `200` et collection (pirates)                                       | `400` si la valeur n'est ni `true` ni `false` |
| `GET /api/pirates/{id}`                                       | `200` et le pirate                                                  | `404`                                         |
| `POST /api/pirates`                                           | `201`, `Location` et pirate créé                                    | `400`                                         |
| `PUT /api/pirates/{id}`                                       | `200` et pirate modifié                                             | `400`, `404`                                  |
| `DELETE /api/pirates/{id}`                                    | `204`                                                               | `404`                                         |
| ===========================                                   | ===============                                                     | =======                                       |
| `GET /api/expeditions`                                        | `200` et les expéditions                                            |                                               |
| `GET /api/expeditions/{id}/assignments`                       | `200` et les affectations de l'expédition                           | `404`                                         |
| `POST /api/expeditions/{id}/cancel`                           | `200` et l'expédition annulée                                       | `404`, `409`                                  |
| `POST /api/expeditions/{id}/assignments`                      | `201` et l'affectation créée                                        | `400`, `404`, `409`, `500`                    |
| `POST /api/expeditions/{id}/assignments?simulateFailure=true` |                                                                     |                                               |
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
## Statuts et format des erreurs

| Statut | Signification |
|---|---|
| 400 | Donnée invalide |
| 404 | Ressource absente (y compris route inconnue) |
| 409 | Conflit métier ou transition refusée |
| 500 | Échec simulé |

Toutes les erreurs utilisent le format suivant:

```json
{
  "title": "Not Found",
  "status": 404,
  "detail": "Pirate not found"
}
```


### Données par défaults:



 Pirates:

| ID  | Name    | Type      | Level | Bounty | Marine | Available |
| --- | ------- | --------- | ----- | ------ | ------ | --------- |
| 1   | Luffy   | fighter   | 1     | 300000 | false  | true      |
| 2   | Zoro    | swordsman | 1     | 250000 | false  | true      |
| 3   | Garp    | fighter   | 1     | 0      | true   | true      |
| 4   | Bellamy | fighter   | 1     | 195000 | false  | true      |

Expeditions:

| Id  | Name      | Capacity | State     |
| --- | --------- | -------- | --------- |
| 1   | Twin Cape | 2        | `open`    |
| 2   | Water 7   | 3        | `planned` |
| 3   | Alabasta  | 7        | `planned` |

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
