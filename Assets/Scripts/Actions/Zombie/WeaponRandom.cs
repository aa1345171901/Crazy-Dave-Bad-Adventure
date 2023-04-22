using Spine;
using Spine.Unity;
using Spine.Unity.AttachmentTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RandomEquip;

public class WeaponRandom : MonoBehaviour
{
    public List<EquipHook> Equippables = new List<EquipHook>();  // 需要替换的骨骼，附件集合
    public List<Sprite> Weapons;  // 可替换的头部图片
    public Material SourceMaterial;   // 源材质

    private Skin equipsSkin;  // 用於更换部件的新建皮肤
    private Material cloneMaterial;
    private SkeletonDataAsset skeletonDataAsset;
    private SkeletonAnimation skeletonAnimation;

    private void Start()
    {
        skeletonAnimation = this.GetComponent<SkeletonAnimation>();
        equipsSkin = new Skin("Equips");
        // 将默认的皮肤添加到新建的皮肤中
        equipsSkin.AddAttachments(skeletonAnimation.Skeleton.Skin);

        cloneMaterial = new Material(SourceMaterial);

        skeletonAnimation.Skeleton.Skin = equipsSkin;
        RefreshSkeletonAttachments();
        skeletonDataAsset = skeletonAnimation.SkeletonDataAsset;

        foreach (var item in Equippables)
        {
            Equip(item);
        }
    }

    public void Equip(EquipHook equipHook)
    {
        var skeletonData = skeletonDataAsset.GetSkeletonData(true);
        int slotIndex = skeletonData.FindSlotIndex(equipHook.slot);
        int random = Random.Range(0, Weapons.Count);
        Sprite sprite = Weapons[random];

        var attachment = GenerateAttachmentFromEquipAsset(slotIndex, equipHook.templateSkin, equipHook.templateAttachment, sprite);
        Equip(slotIndex, equipHook.templateAttachment, attachment);
    }

    Attachment GenerateAttachmentFromEquipAsset(int slotIndex, string templateSkinName, string templateAttachmentName, Sprite sprite)
    {

        Attachment attachment;

        var skeletonData = skeletonDataAsset.GetSkeletonData(true);
        var templateSkin = skeletonData.FindSkin(templateSkinName);
        Attachment templateAttachment = templateSkin.GetAttachment(slotIndex, templateAttachmentName);
        attachment = templateAttachment.GetRemappedClone(sprite, cloneMaterial);

        return attachment;
    }

    public void Equip(int slotIndex, string attachmentName, Attachment attachment)
    {
        equipsSkin.SetAttachment(slotIndex, attachmentName, attachment);
        skeletonAnimation.Skeleton.SetSkin(equipsSkin);
        RefreshSkeletonAttachments();
    }

    void RefreshSkeletonAttachments()
    {
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        skeletonAnimation.AnimationState.Apply(skeletonAnimation.Skeleton); //skeletonAnimation.Update(0);
    }
}
