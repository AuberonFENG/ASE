using UnityEngine;
using Unity.Netcode;
using System.Diagnostics;

namespace XRMultiplayer
{
    /// <summary>
    /// Syncs the material of a plane across the network.
    /// </summary>
    [RequireComponent(typeof(Renderer))]
    public class NetworkedMaterial : NetworkBehaviour
    {
        [SerializeField, Tooltip("List of materials that can be applied to the plane.")]
        private Material[] materials;

        // Networked variable to hold the index of the material
        private NetworkVariable<int> m_NetworkMaterialIndex = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

        // Reference to the Renderer component
        private Renderer m_Renderer;

        private void Awake()
        {
            m_Renderer = GetComponent<Renderer>();
            if (materials == null || materials.Length == 0)
            {
                //Debug.LogError("Materials array is empty! Assign materials in the Inspector.");
            }
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsClient)
            {
                // Apply the current material when a client joins
                UpdateMaterial(m_NetworkMaterialIndex.Value);
            }

            // Subscribe to changes in the NetworkVariable
            m_NetworkMaterialIndex.OnValueChanged += OnMaterialIndexChanged;
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            // Unsubscribe from changes to avoid memory leaks
            m_NetworkMaterialIndex.OnValueChanged -= OnMaterialIndexChanged;
        }

        /// <summary>
        /// Called on all clients when the material index changes.
        /// </summary>
        private void OnMaterialIndexChanged(int oldIndex, int newIndex)
        {
            UpdateMaterial(newIndex);
        }

        /// <summary>
        /// Updates the material based on the given index.
        /// </summary>
        private void UpdateMaterial(int index)
        {
            if (materials != null && index >= 0 && index < materials.Length)
            {
                m_Renderer.material = materials[index];
            }
            else
            {
                //Debug.LogWarning($"Material index {index} is out of range!");
            }
        }

        /// <summary>
        /// Called by a client to request a material change.
        /// </summary>
        /// <param name="newIndex">Index of the material to apply.</param>
        [ServerRpc(RequireOwnership = false)]
        public void ChangeMaterialServerRpc(int newIndex, ulong clientId)
        {
            if (newIndex >= 0 && newIndex < materials.Length)
            {
                m_NetworkMaterialIndex.Value = newIndex;
                NotifyClientsOfChangeClientRpc(newIndex, clientId);
            }
            else
            {
                //Debug.LogWarning($"Client {clientId} requested an invalid material index {newIndex}!");
            }
        }

        /// <summary>
        /// Notifies all clients about the material change.
        /// </summary>
        /// <param name="newIndex">New material index.</param>
        /// <param name="clientId">ID of the client that requested the change.</param>
        [ClientRpc]
        private void NotifyClientsOfChangeClientRpc(int newIndex, ulong clientId)
        {
            if (NetworkManager.Singleton.LocalClientId != clientId)
            {
                UpdateMaterial(newIndex);
            }
        }

        /// <summary>
        /// Allows a client to request a material change locally.
        /// </summary>
        /// <param name="newIndex">Index of the new material.</param>
        public void RequestMaterialChange(int newIndex)
        {
            if (IsClient && IsOwner)
            {
                ChangeMaterialServerRpc(newIndex, NetworkManager.Singleton.LocalClientId);
            }
        }
    }
}
