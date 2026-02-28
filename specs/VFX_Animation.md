# VFX/Animation pipeline: recipes by tags + pooling + event integration


## Цели
- VFX выбираются по TagSet: shape modules + element overlays.
- Никаких “уникальных” VFX в коде: только рецепты + теги.
- Pooling VFX.
- Animation recipes: MotionIntent/Delivery → upper body overlay/additive/IK.

## VFX recipes
### VfxRecipeDefinition (SO)
- RequiredTags
- ForbiddenTags
- ShapeModule (ProjectileTrail, NovaRing, BeamLine, ImpactBurst)
- ElementOverlay (Fire/Cold/Poison)
- VisualCost
- Prefab placeholders

Resolver:
- `IVfxRecipeResolver.Resolve(tags) -> recipe`

Integration:
- Ability executor emits `VfxEvent { position, direction, tags, intensity }`
- Presentation spawns pooled VFX.

## Animation recipes
- `AnimRecipeDefinition`:
  - RequiredTags (Delivery.Melee, Delivery.Projectile, Cast.Channel)
  - Animator state or blend tree
  - UpperBodyMask toggle
  - IK toggle
Integration:
- Ability events: OnCastStart/OnCastEnd trigger Animator.

## Pooling
- unified `Pool<T>` for projectiles/VFX/floating text.

## Тесты
- recipe resolver picks correct recipe
- pooling returns objects
