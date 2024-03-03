using System;
using System.Collections.Generic;
using System.Linq;
using Common.Navigation.Interfaces;
using Common.Units.Exploring;
using Infrastructure.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Common.Navigation.BattleFieldSearchStrategy
{
    public class RandomBattleFieldPositionStrategy : IBattleFieldSearchStrategy
    {
        private readonly float _halfWidth;
        private readonly float _halfHeight;

        public RandomBattleFieldPositionStrategy(float width, float height)
        {
            _halfWidth = width / 2;
            _halfHeight = height / 2;
        }

        public bool TryFindField(IReadOnlyDictionary<Vector2, NavigationCell> cells, out BattleField field)
        {
            float lowestX = cells.Keys.First().x;
            float lowestY = cells.Keys.First().y;
            float highestX = cells.Keys.Last().x;
            float highestY = cells.Keys.Last().y;
            
            float positionX = Random.Range((int)lowestX, (int)highestX + 1);
            float positionY = Random.Range((int)lowestY, (int)highestY + 1);
            int floorWidth = Mathf.FloorToInt(_halfWidth);
            int floorHeight = Mathf.FloorToInt(_halfHeight);

            positionX = (int)Mathf.Clamp(positionX, lowestX + floorWidth, highestX - floorWidth);
            positionY = (int)Mathf.Clamp(positionY, lowestY + floorHeight, highestY - floorHeight);

            Vector2 position = new Vector2(positionX, positionY);
            bool isOccupied = IsAreaOccupied(position);
            
            if (isOccupied == false)
            {
                field = new BattleField(new Vector2(positionX - 0.5f, positionY - 0.5f));

                int ceilWidth = Mathf.CeilToInt(_halfWidth);
                int ceilHeight = Mathf.CeilToInt(_halfHeight);
                float minPositionX = positionX - ceilWidth + 0.5f;
                float maxPositionX = positionX + ceilWidth - 0.5f;
                float minPositionY = positionY - ceilHeight + 0.5f;
                float maxPositionY = positionY + ceilHeight - 0.5f;

                for (float y = minPositionY; y < maxPositionY; y++)
                {
                    for (float x = minPositionX; x < maxPositionX; x++)
                    {
                        field.Add(cells[new Vector2(x, y)]);
                    }
                }

                return true;
            }

            field = null;
            return false;
        }

        private bool IsAreaOccupied(Vector2 position)
        {
            Collider2D[] overlapColliders = new Collider2D[10];
            Vector2 overlapRadius = new Vector2(Constants.BattlefieldMinWidth, Constants.BattlefieldMinHeight);
            int collidersCount = Physics2D.OverlapBoxNonAlloc(position, overlapRadius, 0, overlapColliders);

            if (collidersCount <= 0)
                return false;

            bool isOccupied;
            
            try
            {
                isOccupied = overlapColliders.Any(c => c.isTrigger == false && c.TryGetComponent(out ExploringUnit _) == false);
            }
            catch (Exception e)
            {
                return false;
            }

            return isOccupied;
        }
    }
}