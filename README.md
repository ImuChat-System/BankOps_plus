# BankOps+ -> Application de Supervision Bancaire

## Contexte

**BankOps+** est une application interne simulant un outil de supervision utilisé par une DSI bancaire pour :

- Superviser des **traitements métiers** (batchs, flux, jobs)
- Suivre l'état des **applications critiques**
- Déclarer, qualifier et résoudre des **incidents techniques**
- Garantir la **traçabilité**, la **qualité** et la **continuité de service**

Ce projet démontre ma capacité à maintenir et faire évoluer des applications .NET dans un contexte finance, avec une architecture claire, une gestion d'erreurs robuste, et une documentation complète.

---

## Stack Technique

### Backend

- **C# / ASP.NET Core MVC** (.NET 9.0)
- **Entity Framework Core 9.0** (ORM)
- Architecture en couches (Controllers → Services → Repositories → Data)

### Frontend

- **HTML5** avec vues Razor
- **CSS3** avec Bootstrap 5
- **JavaScript** vanilla

### Base de données

- **SQLite** (portable et simple pour démo)  
  *Compatible SQL Server pour production*

### Sécurité

- **BCrypt.Net** pour hachage des mots de passe

---

## Fonctionnalités

### Dashboard

- **KPIs en temps réel** : Incidents ouverts, incidents critiques, MTTR, jobs en erreur
- **Alertes automatiques** pour jobs en erreur
- **Liste des incidents récents** avec filtre rapide

### Gestion des Incidents

- **CRUD complet** (Create, Read, Update)
- **Filtrage** par statut (Open, InProgress, Resolved) et gravité (Low, Medium, Critical)
- **Traçabilité** : création, résolution, temps de résolution automatique
- **Liaison** avec applications et jobs
- **Assignment** d'incidents aux utilisateurs

### Gestion des Applications

- Catalogue d'applications métiers par environnement (DEV, REC, PROD)
- Responsable, statut global

### Gestion des Jobs

- Suivi des traitements (batchs)
- Fréquence d'exécution (Daily, Weekly, Monthly)
- Historique d'exécution et détection d'erreurs

### 👥 Gestion des Utilisateurs

- Rôles : **Admin**, **Support**, **ReadOnly**
- Authentification sécurisée avec BCrypt

---

## Installation & Lancement

### Prérequis

- **.NET 9.0 SDK** : [Télécharger](https://dotnet.microsoft.com/download/dotnet/9.0)
- Un éditeur : **Visual Studio 2022**, **VS Code**, ou **Rider**

### Étapes

1. **Cloner le repository**

   ```bash
   git clone <votre-repo-url>
   cd BankOps_plus
   ```

2. **Restaurer les dépendances**

   ```bash
   dotnet restore
   ```

3. **Lancer l'application**

   ```bash
   dotnet run
   ```

4. **Accéder à l'application**

   ```
   https://localhost:5001
   ```

La base de données SQLite sera créée automatiquement avec des données de démonstration.

---

## Données de Démonstration

### Utilisateurs

| Email | Mot de passe | Rôle |
|-------|--------------|------|
| <admin@bankops.local> | Admin123! | Admin |
| <support@bankops.local> | Support123! | Support |
| <reader@bankops.local> | Reader123! | ReadOnly |

### Données Incluses

- **5 applications** (Payment Processing, Trading Platform, Risk Management, etc.)
- **5 jobs** avec différents statuts et fréquences
- **3 incidents** (1 résolu, 1 ouvert critique, 1 en cours)
- **3 change requests** (évolutions et bugs)

---

## Structure du Projet

```
BankOps_plus/
├── Controllers/          # Contrôleurs MVC (Dashboard, Incident)
├── Models/               # Modèles de données (User, Application, Job, Incident, ChangeRequest)
├── Views/                # Vues Razor (Dashboard, Incident)
│   ├── Dashboard/
│   ├── Incident/
│   └── Shared/
├── Data/                 # DbContext et migrations
│   ├── BankOpsDbContext.cs
│   └── Scripts/          # Scripts SQL
├── Services/             # Logique métier (à venir)
├── Repositories/         # Accès données (à venir)
├── wwwroot/              # Assets statiques (CSS, JS)
├── docs/                 # Documentation technique (à venir)
├── _docs/                # Documentation originale du projet
└── README.md
```

---

## Prochaines Étapes (Roadmap)

### Phase 2 - Services & Repositories

- [ ] Implémenter la couche Service (logique métier)
- [ ] Implémenter la couche Repository (abstraction données)
- [ ] Injection de dépendances complète

### Phase 3 - Authentification

- [ ] Système de login/logout
- [ ] Gestion des sessions
- [ ] Middleware d'autorisation basée sur les rôles

### Phase 4 - Interopérabilité

- [ ] **API REST** pour export de données
- [ ] **Macro VBA Excel** pour reporting automatique
- [ ] **Page PHP legacy** consommant l'API

### Phase 5 - Documentation

- [ ] `architecture.md` - Architecture système
- [ ] `database-schema.md` - Schéma de base de données
- [ ] `maintenance-guide.md` - Guide de maintenance
- [ ] `known-issues.md` - Problèmes connus et limitations

---

## Points Techniques Démontrés

### Ok Maintenance & Qualité

- Code lisible et bien organisé
- Séparation des responsabilités (MVC pattern)
- Méthodes courtes et explicites
- Noms de variables/méthodes descriptifs

### Ok Gestion des Données

- Modèles avec annotations et validation
- Relations Entity Framework (1-N, optionnelles)
- Seed data pour démarrage rapide
- Gestion des états et transitions

### Ok Architecture

- Pattern Repository (prévu)
- Pattern Service Layer (prévu)
- Dependency Injection
- Configuration externalisée (appsettings.json)

### Ok Frontend

- Interface responsive (Bootstrap 5)
- Badge de statut colorés
- Filtres et recherche
- UX claire et professionnelle

---

## Présentation pour Recruteur (EKXEL)

> *J'ai développé **BankOps+**, une application .NET de supervision bancaire reproduisant un outil interne de DSI, avec gestion d'incidents, monitoring de traitements, et calcul de KPIs (MTTR). Ce projet illustre ma capacité à maintenir et faire évoluer des applications dans un contexte finance, avec une architecture propre, une gestion d'erreurs centralisée, et une documentation complète.*

**Ce qui fait la différence** :

1. **Architecture réaliste** : Pattern MVC avec séparation claire des responsabilités
2. **Contexte métier** : 100% aligné avec la finance (supervision, incidents, jobs)
3. **Qualité** : Code maintenable, commenté, avec gestion d'erreurs
4. **Documentation** : README complet + docs techniques à venir
5. **Interopérabilité** : Prêt pour VBA Excel et PHP legacy (phase 4)

---

## Auteur

Nathan Imogo Imogo  
Développeur .NET Junior  
Projet créé pour démontrer mes compétences en maintenance et évolution d'applications bancaires.

---

## Licence

Ce projet est un projet de démonstration à des fins pédagogiques et de portfolio.
