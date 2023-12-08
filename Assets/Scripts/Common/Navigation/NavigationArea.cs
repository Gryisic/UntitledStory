using System.Collections.Generic;
using Core.Extensions;
using Infrastructure.Utils;
using UnityEngine;

namespace Common.Navigation
{
    public class NavigationArea : MonoBehaviour
    {
        [SerializeField] private NavigationGrid _grid;
        
#if UNITY_EDITOR
        [Header("Editor Draw")]
        [SerializeField] private bool _drawGrid;
        [SerializeField] private bool _drawBattleField;
        [SerializeField] private bool _drawPath;

        private IReadOnlyDictionary<Vector2, NavigationCell> _editorCells;
#endif
        private IReadOnlyList<NavigationCell> _path;
        
        private IReadOnlyDictionary<Vector2, NavigationCell> _cells;

        private BattleField _battleField;
        private Pathfinding _pathfinding;
        
        private void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
            Vector3 position = transform.position;
            
            position = position.SnappedTo(0);
            transform.position = position;

            _grid.Initialize(position);
            _battleField = _grid.GetBattleField();
            _cells = _grid.Cells;
            
            _pathfinding = new Pathfinding(_cells);
        }

        public bool TryGetPath(Vector2 from, Vector2 to, out IReadOnlyList<NavigationCell> path)
        {
            from = from.SnappedTo(0.5f);
            to = to.SnappedTo(0.5f);

            NavigationCell fromCell = _cells[from];
            NavigationCell toCell = _cells[to];
            
            path = GetPath(fromCell, toCell);

            return path != null;
        }
        
        public IReadOnlyList<NavigationCell> GetPathToBattleFieldPosition(Vector2 from, Enums.BattleFieldSide side)
        {
            from = from.SnappedTo(0.5f);

            NavigationCell fromCell = ValidatedCell(from);
            NavigationCell toCell = _battleField.GetRandomCellFromSide(side);
            
            return GetPath(fromCell, toCell);
        }

        public IReadOnlyList<NavigationCell> GetPathToConcreteBattleFieldPosition(Vector2 from, Vector2 to)
        {
            from = from.SnappedTo(0.5f);
            to = to.SnappedTo(0.5f);
            
            NavigationCell fromCell = ValidatedCell(from);
            NavigationCell toCell = ValidatedCell(to);
            
            return GetPath(fromCell, toCell);
        }

        private NavigationCell ValidatedCell(Vector2 from)
        {
            if (_cells.TryGetValue(from, out NavigationCell fromCell) == false)
            {
                _grid.AddAdditionalCells(from);

                fromCell = _cells[from];
            }

            return fromCell;
        }
        
        private IReadOnlyList<NavigationCell> GetPath(NavigationCell fromCell, NavigationCell toCell)
        {
            IReadOnlyList<NavigationCell> path = _pathfinding.GetPath(fromCell, toCell);
            
            _grid.ClearCellsData();

            _path = path;
            return path;
        }

#if UNITY_EDITOR
        [ContextMenu("GetDrawCells")]
        private void GetDrawCells()
        {
            _grid.Initialize(transform.position.SnappedTo(0));

            _editorCells = _grid.Cells;
        }
        
        private void OnDrawGizmosSelected()
        {
            if (_editorCells == null && _grid != null) 
                GetDrawCells();

            if (_editorCells is not { Count: > 0 })
                return;

            if (_drawGrid)
                DrawGrid();
            
            if (_battleField == null)
                return;
            
            if (_drawBattleField)
                DrawChunks();

            if (_path == null)
                return;
            
            if (_drawPath && _path.Count > 0)
                DrawPath();
        }

        private void DrawChunks()
        {
            Color color = Color.black;

            foreach (var cell in _battleField.Cells)
            {
                Vector2 leftBottomPosition = new Vector2(cell.Position.x - 0.5f, cell.Position.y - 0.5f);
                Vector2 rightTopPosition = new Vector2(cell.Position.x + 0.5f, cell.Position.y + 0.5f);

                DrawCell(leftBottomPosition, rightTopPosition, color);
            }
        }

        private void DrawGrid()
        {
            foreach (var cell in _editorCells.Values)
            {
                Vector2 leftBottomPosition = new Vector2(cell.Position.x - 0.5f, cell.Position.y - 0.5f);
                Vector2 rightTopPosition = new Vector2(cell.Position.x + 0.5f, cell.Position.y + 0.5f);
                Color color = cell.IsOccupied ? Color.red : Color.white;

                DrawCell(leftBottomPosition, rightTopPosition, color);
            }
        }

        private void DrawCell(Vector2 leftBottomPosition, Vector2 rightTopPosition, Color color)
        {
            Gizmos.color = color;
            
            Gizmos.DrawLine(leftBottomPosition, new Vector3(leftBottomPosition.x, rightTopPosition.y));
            Gizmos.DrawLine(leftBottomPosition, new Vector3(rightTopPosition.x, leftBottomPosition.y));
            Gizmos.DrawLine(rightTopPosition, new Vector3(leftBottomPosition.x, rightTopPosition.y));
            Gizmos.DrawLine(rightTopPosition, new Vector3(rightTopPosition.x, leftBottomPosition.y));
        }
        
        private void DrawPath()
        {
            Gizmos.color = Color.cyan;

            for (var i = 0; i < _path.Count; i++)
            {
                NavigationCell cell = _path[i];

                if (i + 1 >= _path.Count)
                    return;
                
                Gizmos.DrawLine(cell.Position, _path[i + 1].Position);
            }
        }
#endif
    }
}