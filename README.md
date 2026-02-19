# 🐛 GestionAnomalie_AyaQUARINE

Une application web complète de gestion d'anomalies développée avec ASP.NET Core 9.0, permettant le suivi et la résolution des bugs et incidents dans un environnement collaboratif.

## 📋 Table des matières

- [Aperçu du projet](#aperçu-du-projet)
- [Fonctionnalités principales](#fonctionnalités-principales)
- [Architecture technique](#architecture-technique)
- [Modèle de données](#modèle-de-données)
- [Système de rôles et permissions](#système-de-rôles-et-permissions)
- [Installation et configuration](#installation-et-configuration)
- [Structure du projet](#structure-du-projet)
- [Technologies utilisées](#technologies-utilisées)

## 🎯 Aperçu du projet

**GestionAnomalie_AyaQUARINE** est une application web de gestion d'anomalies (bug tracking system) conçue pour les équipes de développement et de maintenance informatique. Elle permet de :

- ✅ Déclarer et tracer les anomalies/incidents
- 📊 Suivre l'évolution des corrections via un workflow de statuts
- 👥 Gérer l'affectation des tâches selon les rôles utilisateur
- 🗂️ Organiser les anomalies par projets avec priorisation
- 💬 Communiquer via un système de commentaires
- 📎 Joindre des pièces justificatives

## 🚀 Fonctionnalités principales

### 🔐 Gestion des utilisateurs
- **Authentification** par cookies avec sessions persistantes (24h)
- **4 niveaux de rôles** : Administrateur, Responsable, Technicien, Utilisateur
- **Autorisations granulaires** selon le rôle et le contexte

### 📝 Gestion des anomalies
- **Création d'anomalies** avec classification (type, priorité, statut)
- **Assignation automatique ou manuelle** aux techniciens
- **Suivi du cycle de vie** : Nouvelle → En cours → En attente → Résolue/Rejetée
- **Pièces jointes** pour documenter les problèmes
- **Commentaires** pour la communication d'équipe

### 📊 Tableau de bord
- **Vue d'ensemble personnalisée** selon le rôle utilisateur
- **Métriques** : total anomalies, en cours, résolues
- **Filtrage intelligent** par projet, statut, assignation

### 🗂️ Gestion des projets
- **Organisation par projets** avec responsables dédiés
- **Planification** avec dates de début et fin
- **Suivi des anomalies** par projet

## 🏗️ Architecture technique

### Pattern architectural
- **ASP.NET Core MVC** avec architecture en couches
- **Repository Pattern** pour l'accès aux données
- **Dependency Injection** natif d'ASP.NET Core
- **Services métier** pour la logique applicative

### Couches applicatives
```
┌─────────────────┐
│   Controllers   │ ← Contrôleurs MVC (API + Vues)
├─────────────────┤
│    Services     │ ← Logique métier et règles
├─────────────────┤
│  Repositories   │ ← Accès aux données (CRUD)
├─────────────────┤
│ Entity Framework│ ← ORM et mapping objet-relationnel
├─────────────────┤
│   SQL Server    │ ← Base de données relationnelle
└─────────────────┘
```

## 🗂️ Modèle de données

### Entités principales

#### 👤 **User**
```csharp
- Id : int (PK)
- Nom : string (50 caractères)
- Prenom : string (50 caractères)
- Email : string (unique)
- MotDePasse : string (hashé en production)
- Role : RoleUser (Enum)
- DateCreation : DateTime
```

#### 🐛 **Anomalie**
```csharp
- Id : int (PK)
- Titre : string (100 caractères)
- Description : string (texte long)
- DateCloture : DateTime? (nullable)
- CreateurId : int (FK → User)
- AssigneeId : int? (FK → User, nullable)
- ProjetId : int (FK → Projet)
- TypeAnomalieId : int (FK → TypeAnomalie)
- PrioriteId : int (FK → Priorite)
- StatutId : int (FK → Statut)
```

#### 📋 **Projet**
```csharp
- Id : int (PK)
- Nom : string (100 caractères)
- Description : string (texte long)
- DateDebut : DateTime
- DateFin : DateTime
- ResponsableId : int? (FK → User, nullable)
```

### Entités de référence

#### 🎯 **Priorite**
- **Niveaux** : Faible (1) → Critique (4)
- **Code couleur** : Affichage visuel (#hex)
- **Exemples** : 
  - Faible (#28a745)
  - Moyenne (#ffc107)
  - Haute (#fd7e14)
  - Critique (#dc3545)

#### 📊 **Statut**
- **Types** : Nouvelle, En cours, En attente, Résolue, Rejetée
- **EstFinal** : Indique si le statut clôture l'anomalie

#### 🏷️ **TypeAnomalie**
- **Catégories** : Bug, Matériel, Demande (nouvelle fonctionnalité)

## 👥 Système de rôles et permissions

### 🛡️ Hiérarchie des rôles

#### 🔴 **Administrateur** (Niveau 0)
- ✅ **Accès total** : Peut tout voir et modifier
- ✅ **Gestion utilisateurs** : CRUD sur tous les comptes
- ✅ **Configuration système** : Types, priorités, statuts
- ✅ **Vue globale** : Toutes les anomalies, tous projets

#### 🟡 **Responsable** (Niveau 1)
- ✅ **Gestion de projets** : Création et modification des projets
- ✅ **Anomalies de ses projets** : Vue et modification limitée aux projets assignés
- ✅ **Assignation** : Peut affecter des techniciens aux anomalies
- ❌ **Limitations** : Pas d'accès aux autres projets

#### 🔵 **Technicien** (Niveau 2)
- ✅ **Anomalies assignées** : Vue et modification des anomalies qui lui sont attribuées
- ✅ **Résolution** : Peut changer le statut vers "Résolue"
- ✅ **Commentaires** : Communication avec l'équipe
- ❌ **Limitations** : Ne peut pas créer d'anomalies (généralement)

#### 🟢 **Utilisateur** (Niveau 3)
- ✅ **Déclaration** : Peut créer des anomalies
- ✅ **Ses anomalies** : Vue et modification tant qu'elles ne sont pas prises en charge
- ✅ **Suivi** : Peut consulter l'évolution de ses déclarations
- ❌ **Limitations** : Perd les droits de modification une fois l'anomalie assignée

### 🔒 Règles de sécurité

```csharp
// Exemples de logique d'autorisation
public bool PeutModifierAnomalie(string role, Anomalie anomalie, int userId)
{
    if (role == "Administrateur") return true;
    
    if (role == "Responsable")
        return anomalie.Projet.ResponsableId == userId;
    
    if (role == "Utilisateur")
        return anomalie.CreateurId == userId && anomalie.AssigneeId == null;
        
    return false;
}
```

## ⚙️ Installation et configuration

### Prérequis
- **.NET 9.0 SDK**
- **SQL Server** (LocalDB ou instance complète)
- **Visual Studio 2022** ou VS Code
- **Git** pour le versioning

### Configuration base de données

1. **Chaîne de connexion** (appsettings.json) :
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=EMSI_or_g8_anomalie;User Id=sa;Password=Pa$$w0rd123;TrustServerCertificate=True;MultipleActiveResultSets=True"
  }
}
```

2. **Migrations Entity Framework** :
```bash
# Créer une migration
dotnet ef migrations add NomDeLaMigration

# Appliquer les migrations
dotnet ef database update
```

3. **Données de démonstration** :
Le système inclut un `DbSeeder` qui génère automatiquement des données réalistes au démarrage :
- **5 utilisateurs représentatifs** avec des profils professionnels
- **3 projets d'entreprise** avec responsables assignés
- **4 anomalies détaillées** couvrant différents scénarios techniques  
- **6 commentaires contextuels** simulant la collaboration d'équipe

### Comptes de démonstration

| Email | Nom complet | Rôle | Mot de passe |
|-------|-------------|------|--------------|
| admin@company.com | Sarah Martinez | Administrateur | 123456 |
| m.johnson@company.com | Michael Johnson | Responsable | 123456 |
| d.chen@company.com | David Chen | Technicien | 123456 |
| e.williams@company.com | Emma Williams | Technicien | 123456 |
| r.brown@company.com | Robert Brown | Utilisateur | 123456 |

### Lancement de l'application

```bash
# Restaurer les packages
dotnet restore

# Compiler le projet
dotnet build

# Lancer l'application
dotnet run
```

L'application sera accessible sur : `https://localhost:7xxx`

## 📁 Structure du projet

```
Web_QA/
├── 📂 Controllers/          # Contrôleurs MVC
│   ├── AccountController.cs       # Authentification
│   ├── AnomalieController.cs      # Gestion des anomalies
│   ├── HomeController.cs          # Dashboard
│   └── ProjetController.cs        # Gestion des projets
│
├── 📂 Data/                # Contexte de base de données
│   ├── AppDbContext.cs            # Configuration EF Core
│   └── DbSeeder.cs                # Données de test
│
├── 📂 Entities/            # Modèles de domaine
│   ├── Anomalie.cs               # Entité principale
│   ├── User.cs                   # Utilisateurs
│   ├── Projet.cs                 # Projets
│   ├── Base.cs                   # Classe de base
│   └── RoleUser.cs               # Énumération des rôles
│
├── 📂 Repositories/        # Couche d'accès aux données
│   ├── Interfaces/               # Contrats repository
│   └── *Repository.cs            # Implémentations CRUD
│
├── 📂 Services/            # Logique métier
│   ├── Interfaces/               # Contrats services
│   ├── AnomalieService.cs        # Règles métier anomalies
│   ├── AutorisationService.cs    # Gestion des permissions
│   └── AuthenticationService.cs   # Logique d'authentification
│
├── 📂 Models/              # ViewModels pour les vues
│   ├── AnomalieCreateViewModel.cs
│   ├── DashboardViewModel.cs
│   └── LoginViewModel.cs
│
├── 📂 Views/               # Templates Razor
│   ├── Account/                  # Vues d'authentification
│   ├── Anomalie/                 # CRUD anomalies
│   ├── Home/                     # Dashboard
│   └── Shared/                   # Layouts partagés
│
├── 📂 Migrations/          # Historique des schémas DB
└── 📂 wwwroot/             # Ressources statiques (CSS, JS, images)
```

## 🛠️ Technologies utilisées

### Backend
- **ASP.NET Core 9.0** - Framework web principal
- **Entity Framework Core 9.0** - ORM pour SQL Server
- **Microsoft.AspNetCore.Authentication.Cookies** - Authentification

### Base de données
- **SQL Server** - SGBD relationnel
- **Entity Framework Migrations** - Gestion du schéma

### Frontend
- **Razor Pages** - Moteur de templates
- **Bootstrap** (inclus via libman.json)
- **HTML5/CSS3/JavaScript** - Technologies web standards

### Architecture et patterns
- **Repository Pattern** - Séparation de la logique d'accès aux données
- **Dependency Injection** - Inversion de contrôle
- **MVC Pattern** - Séparation des responsabilités
- **Claims-based Authentication** - Système d'autorisation moderne

## 🔒 Sécurité et considérations de production

### ⚠️ Note importante pour la production
Ce projet est **à des fins de démonstration uniquement**. Pour un déploiement en production, les améliorations suivantes sont **obligatoires** :

- 🔐 **Hachage des mots de passe** avec bcrypt ou Argon2
- 🔑 **Variables d'environnement** pour les chaînes de connexion sensibles
- 🛡️ **Validation et assainissement** des entrées utilisateur
- 📜 **Journalisation et audit trail** pour la traçabilité
- 🔒 **HTTPS forcé** et en-têtes de sécurité
- ⚡ **Limitation du débit** (rate limiting) pour les API

### Configuration de production recommandée
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "#{CONNECTION_STRING}#"
  },
  "Security": {
    "RequireHttps": true,
    "PasswordHashingIterations": 10000
  }
}
```

## 🎨 Fonctionnalités avancées

### 🔄 Workflow des anomalies
```
Nouvelle → En cours → En attente ↘
   ↓                              → Résolue
   └─────────────────────────────→ Rejetée
```

### 💡 Règles métier implémentées
- **Visibilité contextuelle** : Les utilisateurs ne voient que leurs anomalies/projets
- **Assignation intelligente** : Les responsables assignent dans leurs projets uniquement
- **Historique des modifications** : Traçabilité via DateModification
- **Validation des permissions** : Contrôles d'accès à chaque action

### 🎯 Points d'amélioration futurs
- [ ] **Notifications push** lors de changements d'état
- [ ] **Historique complet** des modifications avec audit trail
- [ ] **API REST** pour intégrations externes
- [ ] **Rapports avancés** et export Excel/PDF
- [ ] **Tableau de bord temps réel** avec SignalR
- [ ] **Tests unitaires et d'intégration** complets
- [ ] **Containerisation Docker** pour le déploiement

## 🚀 Démo en ligne

> 🌐 **Version de démonstration :** [À venir - Déploiement Azure prévu]  
> 📱 **Interface responsive :** Optimisée pour desktop et mobile  
> 🔍 **Données réalistes :** Scénarios d'entreprise authentiques

## 📋 Fonctionnalités démontrées

✅ **Authentification multi-rôles** avec autorisations contextuelles  
✅ **CRUD complet** pour anomalies, projets et utilisateurs  
✅ **Workflow métier** avec statuts et transitions  
✅ **Interface utilisateur moderne** avec Bootstrap 5  
✅ **Architecture propre** suivant les bonnes pratiques .NET  
✅ **Base de code maintenable** avec séparation des responsabilités

---

## 👨‍💻 À propos du développeur

**Aya QUARINE** 

> 🎓 **Formation :** Ingénieur Informatique - 4ème année  
> 💼 **Spécialités :** ASP.NET Core, Entity Framework, Architecture logicielle  
> 🌟 **Objectif :** Créer des solutions web robustes et évolutives  

### 🔗 Contact & Liens
- 💼 **LinkedIn :** https://www.linkedin.com/in/aya-quarine-b6b5532a5/
- 📧 **Email :** quarineaya1@gmail.com
- 🐱 **GitHub :** (https://github.com/AyaQuarine)

---

## 📄 Licence

Ce projet est développé à des fins éducatives et de démonstration. Libre d'utilisation pour l'apprentissage et les projets académiques.

**⚠️ Remarque importante :** Ce code est optimisé pour la démonstration et l'apprentissage. Pour une utilisation en production, veuillez implémenter les mesures de sécurité appropriées mentionnées ci-dessus.

---

<div align="center">

**⭐ Si ce projet vous a aidé, n'hésitez pas à lui donner une étoile ! ⭐**

*Développé avec ❤️ en ASP.NET Core*

</div>