﻿using System;

namespace ModLibrary
{
    /// <summary>
    /// Defines all bit field values for restricting <see cref="FirstPersonMover"/> input, to restrict a <see cref="FirstPersonMover"/>s input, see the <see cref="CharacterInputRestrictor"/> class
    /// </summary>
    [Flags]
    public enum InputRestrictions
    {
        /// <summary>
        /// Disables the jumping input for the <see cref="FirstPersonMover"/> the bitfield is applied to
        /// </summary>
        Jump = 1 << 0,

        /// <summary>
        /// Disables the vertical cursor movement for the <see cref="FirstPersonMover"/> the bitfield is applied to
        /// </summary>
        VerticalCursorMovement = 1 << 1,

        /// <summary>
        /// Disables the horizontal cursor movement for the <see cref="FirstPersonMover"/> the bitfield is applied to
        /// </summary>
        HorizontalCursorMovement = 1 << 2,

        /// <summary>
        /// Disables the attack key down (swing melee weapon) input for the <see cref="FirstPersonMover"/> the bitfield is applied to
        /// </summary>
        AttackKeyDown = 1 << 3,

        /// <summary>
        /// Disables the attack key up (release arrow or throw grenade) inputs for the <see cref="FirstPersonMover"/> the bitfield is applied to
        /// </summary>
        AttackKeyUp = 1 << 4,

        /// <summary>
        /// Disables the attack key held (aiming with the bow or the grenade) inputs for the <see cref="FirstPersonMover"/> the bitfield is applied to
        /// </summary>
        AttackKeyHeld = 1 << 5,

        /// <summary>
        /// Disables the secondary attack key down (kicking) input for the <see cref="FirstPersonMover"/> the bitfield is applied to
        /// </summary>
        SecondAttackKeyDown = 1 << 6,

        /// <summary>
        /// Disables the secondary attack key up (stop jetpack-kicking) input for the <see cref="FirstPersonMover"/> the bitfield is applied to
        /// </summary>
        SecondAttackKeyUp = 1 << 7,

        /// <summary>
        /// Disables the secondary attack key held (jetpack-kicking) input for the <see cref="FirstPersonMover"/> the bitfield is applied to
        /// </summary>
        SecondAttackKeyHeld = 1 << 8,

        /// <summary>
        /// Disables the jetpack input for the <see cref="FirstPersonMover"/> the bitfield is applied to
        /// </summary>
        JetpackKeyHeld = 1 << 9,

        /// <summary>
        /// Disables the scroll wheel delta (rotating arrows) input for the <see cref="FirstPersonMover"/> the bitfield is applied to
        /// </summary>
        ScrollWheelDelta = 1 << 10,

        /// <summary>
        /// Disables the use ability input for the <see cref="FirstPersonMover"/> the bitfield is applied to
        /// </summary>
        UseAbilityKeyDown = 1 << 11,

        /// <summary>
        /// Disables the use ability continuously (flame breath) input for the <see cref="FirstPersonMover"/> the bitfield is applied to
        /// </summary>
        UseAbilityKeyHeld = 1 << 12,

        /// <summary>
        /// Disables the next ability key down input for the <see cref="FirstPersonMover"/> the bitfield is applied to
        /// </summary>
        NextAbilityKeyDown = 1 << 13,

        /// <summary>
        /// Disables the use key down input for the <see cref="FirstPersonMover"/> the bitfield is applied to
        /// </summary>
        UseKeyDown = 1 << 14,

        /// <summary>
        /// Disables the transfer consciousness key down input for the <see cref="FirstPersonMover"/> the bitfield is applied to
        /// </summary>
        TransferConsciousnessKeyDown = 1 << 15,

        /// <summary>
        /// Disables the switch to weapon 1 key down (sword) input for the <see cref="FirstPersonMover"/> the bitfield is applied to
        /// </summary>
        SwitchToWeapon1KeyDown = 1 << 16,

        /// <summary>
        /// Disables the switch to weapon 2 key down (bow) input for the <see cref="FirstPersonMover"/> the bitfield is applied to
        /// </summary>
        SwitchToWeapon2KeyDown = 1 << 17,

        /// <summary>
        /// Disables the switch to weapon 3 key down (hammer) input for the <see cref="FirstPersonMover"/> the bitfield is applied to
        /// </summary>
        SwitchToWeapon3KeyDown = 1 << 18,

        /// <summary>
        /// Disables the switch to weapon 4 key down (spear) input for the <see cref="FirstPersonMover"/> the bitfield is applied to
        /// </summary>
        SwitchToWeapon4KeyDown = 1 << 19,

        /// <summary>
        /// Disables the switch to weapon 5 key down (grenade) input for the <see cref="FirstPersonMover"/> the bitfield is applied to
        /// </summary>
        SwitchToWeapon5KeyDown = 1 << 20,

        /// <summary>
        /// Disables the next weapon key down input for the <see cref="FirstPersonMover"/> the bitfield is applied to
        /// </summary>
        NextWeaponKeyDown = 1 << 21,

        /// <summary>
        /// Disables the vertical movement (walk forwards and backwards) input for the <see cref="FirstPersonMover"/> the bitfield is applied to
        /// </summary>
        VerticalMovement = 1 << 22,

        /// <summary>
        /// Disables the horizontal movement (walk left and right) input for the <see cref="FirstPersonMover"/> the bitfield is applied to
        /// </summary>
        HorizontalMovement = 1 << 23,

        /// <summary>
        /// Disables the emote key held (emote menu) input for the <see cref="FirstPersonMover"/> the bitfield is applied to
        /// </summary>
        EmoteKeyHeld = 1 << 24
    }
}
