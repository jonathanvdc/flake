using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Flake
{
    /// <summary>
    /// Represents a directed graph.
    /// </summary>
    public sealed class Graph<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Flake.Graph&lt;T&gt;"/> class.
        /// </summary>
        public Graph()
        {
            this.forwardEdges = new Dictionary<T, HashSet<T>>();
            this.backwardEdges = new Dictionary<T, HashSet<T>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Flake.Graph&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="Other">The graph to copy.</param>
        public Graph(Graph<T> Other)
        {
            this.forwardEdges = CopySetDictionary(Other.forwardEdges);
            this.backwardEdges = CopySetDictionary(Other.backwardEdges);
        }

        [JsonProperty("edges")]
        private Dictionary<T, HashSet<T>> Edges
        {
            get
            {
                return forwardEdges;
            }
            set
            {
                // Set the forward edges dictionary.
                forwardEdges = value;

                // Regenerate the backward edges dictionary.
                backwardEdges = new Dictionary<T, HashSet<T>>();

                // Create sets for all vertices.
                foreach (var key in forwardEdges.Keys)
                {
                    backwardEdges.Add(key, new HashSet<T>());
                }

                // Populate the sets.
                foreach (var kvPair in forwardEdges)
                {
                    foreach (var val in kvPair.Value)
                    {
                        backwardEdges[val].Add(kvPair.Key);
                    }
                }
            }
        }

        [JsonIgnore]
        private Dictionary<T, HashSet<T>> forwardEdges;

        [JsonIgnore]
        private Dictionary<T, HashSet<T>> backwardEdges;

        private static Dictionary<T, HashSet<T>> CopySetDictionary(
            Dictionary<T, HashSet<T>> Dictionary)
        {
            var results = new Dictionary<T, HashSet<T>>();
            foreach (var kvPair in Dictionary)
                results[kvPair.Key] = new HashSet<T>(kvPair.Value);

            return results;
        }

        /// <summary>
        /// Gets all vertices in the graph.
        /// </summary>
        /// <value>The vertices.</value>
        [JsonIgnore]
        public IEnumerable<T> Vertices
        {
            get { return forwardEdges.Keys; }
        }

        /// <summary>
        /// Gets the set of vertices to which the given vertex
        /// has a directed edge.
        /// </summary>
        /// <param name="Value">The source vertex.</param>
        public IEnumerable<T> this[T Value]
        {
            get { return GetOutgoingEdges(Value); }
        }

        /// <summary>
        /// Gets the set of vertices to which the given vertex
        /// has a directed edge.
        /// </summary>
        /// <param name="Value">The source vertex.</param>
        public IEnumerable<T> GetOutgoingEdges(T Value)
        {
            return forwardEdges[Value];
        }

        /// <summary>
        /// Gets the set of vertices that have directed edges
        /// to this vertex.
        /// </summary>
        /// <param name="Value">The target vertex.</param>
        public IEnumerable<T> GetIncomingEdges(T Value)
        {
            return backwardEdges[Value];
        }

        /// <summary>
        /// Determines if the graph contains the given vertex.
        /// </summary>
        /// <returns><c>true</c>, if the graph contains the given vertex, <c>false</c> otherwise.</returns>
        /// <param name="Value">The vertex.</param>
        public bool ContainsVertex(T Value)
        {
            return forwardEdges.ContainsKey(Value);
        }

        /// <summary>
        /// Determines if the graph contains an edge from the first
        /// vertex to the second.
        /// </summary>
        /// <returns><c>true</c>, if the graph contains an edge from the first
        /// vertex to the second, <c>false</c> otherwise.</returns>
        /// <param name="From">The source vertex.</param>
        /// <param name="To">The target vertex.</param>
        public bool ContainsEdge(T From, T To)
        {
            return ContainsVertex(From)
                && forwardEdges[From].Contains(To);
        }

        /// <summary>
        /// Adds the given vertex to the graph, if it is not
        /// in the graph already.
        /// </summary>
        /// <returns><c>true</c>, if the vertex was added, <c>false</c> otherwise.</returns>
        /// <param name="Value">The vertex to add.</param>
        public bool AddVertex(T Value)
        {
            if (!ContainsVertex(Value))
            {
                forwardEdges[Value] = new HashSet<T>();
                backwardEdges[Value] = new HashSet<T>();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Removes the given vertex from the graph, along with all
        /// edges that contain it.
        /// </summary>
        /// <returns><c>true</c>, if a vertex was removed, <c>false</c> otherwise.</returns>
        /// <param name="Value">The vertex to remove.</param>
        public bool RemoveVertex(T Value)
        {
            if (forwardEdges.Remove(Value))
            {
                // Erase edges from all other vertices if they
                // point to the vertex to remove.
                foreach (var fromVertex in backwardEdges[Value])
                {
                    forwardEdges[fromVertex].Remove(Value);
                }
                backwardEdges.Remove(Value);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Adds a directed edge from the first vertex to the second 
        /// vertex.
        /// </summary>
        /// <returns><c>true</c>, if an edge was added, <c>false</c> otherwise.</returns>
        /// <param name="From">The source vertex.</param>
        /// <param name="To">The target vertex.</param>
        public bool AddEdge(T From, T To)
        {
            AddVertex(From);
            AddVertex(To);

            if (forwardEdges[From].Add(To))
            {
                backwardEdges[To].Add(From);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Removes a directed edge from the first vertex to the second
        /// vertex.
        /// </summary>
        /// <returns><c>true</c>, if an edge was removed, <c>false</c> otherwise.</returns>
        /// <param name="From">The source vertex.</param>
        /// <param name="To">The target vertex.</param>
        public bool RemoveEdge(T From, T To)
        {
            if (!ContainsVertex(From) || !ContainsVertex(To))
            {
                return false;
            }
            else if (forwardEdges[From].Remove(To))
            {
                backwardEdges[To].Remove(From);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Adds all edges and vertices from the given graph to
        /// this graph.
        /// </summary>
        /// <param name="Other">
        /// The graph that will be added to this graph.
        /// </param>
        public void UnionWith(Graph<T> Other)
        {
            foreach (var edge in Other.forwardEdges)
            {
                AddVertex(edge.Key);
                foreach (var v in edge.Value)
                {
                    AddEdge(edge.Key, v);
                }
            }
        }
    }
}

