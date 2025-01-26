# CanteenBillingSystem
# Cantine Kata

## Feature 1 - Gestion de facturation d'un repas

### Fonctionnalités

Votre programme permet de :

- **Créditer** le compte d'un client de son budget pour la cantine.
- **Payer** le plateau repas du client, incluant :
  - **Débiter** le compte du montant du repas.
  - Générer un **"ticket client"** dans lequel figurent :
    - Le détail des produits ajoutés au plateau.
    - Le total à régler par le client.

---

### Règles

#### **Règle 1** : Prix fixe du plateau
Chaque client peut avoir un prix de plateau fixe de **10€** s'il est composé comme suit :
- 1 Entrée
- 1 Plat
- 1 Dessert
- 1 Pain

#### **Règle 2** : Suppléments
Chaque produit peut être acheté via un supplément, avec les tarifs suivants :
- **Boisson** : 1€
- **Fromage** : 1€
- **Pain** : 0,40€
- **Petit Salade Bar** : 4€
- **Grand Salade Bar** : 6€
- **Portion de fruit** : 1€
- **Entrée supplémentaire** : 3€
- **Plat supplémentaire** : 6€
- **Dessert supplémentaire** : 3€

#### **Règle 3** : Prise en charge employeur
Le ticket du client prend en compte la prise en charge employeur selon le type de client :
- **Client Interne** : 7.5€ de prise en charge.
- **Client Prestataire** : 6€ de prise en charge.
- **Client VIP** : 100%.
- **Client Stagiaire** : 10€.
- **Client Visiteur** : Pas de prise en charge.

#### **Règle 4** : Bloquer les paiements insuffisants
Le paiement du repas devra être bloqué si le montant à débiter est supérieur au crédit restant sur le compte du client, sauf pour :
- **Internes** : Débit autorisé même en cas de découvert.
- **VIP** : Débit autorisé même en cas de découvert.

---

## Implémentation Actuelle et Points à Clarifier

### Ce qui est implémenté

- Les règles décrites ci-dessus ont été prises en compte dans cette version.
- **Menu à prix fixe** : Un plateau composé d'une entrée, d'un plat, d'un dessert, et d'un pain est considéré comme un menu à **10€**.
- **Prise en charge VIP** : Les clients VIP bénéficient d'une prise en charge à 100% de tout ce qu'ils prennent (incluant les suppléments).
- **Débit de compte** : L'endpoint de paiement débite le montant dû directement du compte du client.

### Points à clarifier

1. **Menu sans pain** :
   - Si le plateau contient une entrée, un plat et un dessert, mais pas de pain, doit-il être considéré comme un menu à prix fixe ?
   - **Décision actuelle** : Cette version **ne considère pas** ce plateau comme un menu à 10€.

2. **Prise en charge VIP à 100%** :
   - Est-ce que la prise en charge VIP concerne uniquement le prix du menu fixe (10€) ou tout ce que le client consomme ?
   - **Décision actuelle** : Cette version implémente une prise en charge à 100% de **tout ce que le client a consommé**, même en cas de découvert.

3. **Total à régler par le client** :
   - Dans l'endpoint de paiement, le "total à régler par le client" signifie-t-il :
     - Le montant effectivement débité du compte du client ?
     - Ou le montant restant à payer en cas de découvert ?
   - **Décision actuelle** : Cette version affiche le **montant débité du compte du client**.

---

Ces points peuvent être ajustés ou clarifiés dans les versions futures pour mieux répondre aux exigences métier ou spécifications complémentaires.
