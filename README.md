# Crazy Dave Bad Adventure
# 疯狂戴夫的不妙冒险：
素材取自植物大战僵尸1代
将植物大战僵尸制作成rougelike类型的游戏，类似于土豆兄弟，但是主要的攻击手段为植物的培养，次要攻击手段为戴夫的平底锅以及各种购买的道具的配合。
僵尸和戴夫使用的是Spine进行动画的制作，植物由于动作较少则使用帧动画。

# 修复以及优化
修复毁灭菇，窝瓜可能秒杀僵王bug
移动端UI进行适配

增加移动端，对移动端进行特定优化，场景分辨率暂未调整
优化战斗卡槽的触发动画，解决有时不能点击金币阳光问题
修复手动植物卖出，开槽未更新问题
花园增加卡片点击取消出战

解决僵王死后受损部位恢复正常问题
解决水花盆卖出后，有花盆无法种植的bug
打打僵王模式增加默认阳光
解决打打僵王模式重新开始是重新开始冒险的问题
修复打打僵王模式失败会删除原存档问题
新增打打僵王模式

火爆辣椒不能去掉冰面
怪物强度调整，
主角血量和无敌时间稍微减少
修复僵尸死后，道具（铁桶等）复原的问题

# 后期调整与优化测试
高坚果bug,减速每变色似乎有bug,红眼巨人还没有，10波巨人有点弱，血量可增加一点击杀巨人需要将时间缩短
碰撞伤害可回调一点
12波有bug
寒冰菇冰冻效果时还能移动攻击，动画bug
卡片保存未保存全bug
猫尾草冷却点满过于bt，杨桃子弹
10波之后可进行大增强，怪物有点少
僵王死后循环摇旗子

# 关卡设计
前4波发育关，只生成普通僵尸，僵尸数量逐渐增多，增大金币获取数量
第五波普通僵尸数量减少，增加撑杆跳僵尸
第六波增加路障僵尸，生成的速度变少，僵尸种类依次增加，
前10波每次最多6个怪
第10、15波为精英怪
11波后每波生成怪物增加1
到19波时为13只
无敌时间增加至0.5s

# 增加僵尸2023.4.18 - 4.24
增加旗帜僵尸、铁桶僵尸、路障僵尸、铁栅门僵尸，  磁力菇完善吸铁功能
增加橄榄球僵尸，读报僵尸
增加撑杆跳僵尸，气球僵尸
增加冰车僵尸，投篮僵尸
增加巨人僵尸，金盏花在商店时仍可产生金币，不修改当彩蛋，反正金币最大存在时间
增加boss
增加银币掉落概率，减速金币，钻石价值

# 增加道具2023.4.17
# 花园增加出售植物、移动植物、铲掉泥土功能
增加钥匙： 可免费刷新一次商品，   白色   25
增加卡槽：增加出战卡片的上限，最大8  蓝色  55
增加铲子：可以铲掉花园的泥土    蓝色  52
书：增加植物学  +15    蓝色  50
奖杯：增加金币 +20，幸运 +3   蓝色   54
推土车 增加植物学10，力量5，护甲5，降速度-5    紫色  110
锁  增加伤害 + 5，暴击率 + 5，降低攻击速度  -3  蓝色   45
留声机  增加植物学 + 10，肾上腺素 + 5，最大生命值 + 5，降低护甲 - 3  蓝色  50
三明治  增加最大生命值 10，生命恢复 5   蓝色  65
钱袋  增加金币 10   白色  20

# 2023.4.15-17
杨桃： 基础伤害， 伤害百分比，冷却时间，子弹速度，溅射伤害（升级增加百分比），幸运
向日葵、双子向日葵：阳光（每级+25），幸运，冷却时间，生成阳光质量（默认25，每级+5，大小变大10%）,掉落双倍概率（每级+3，默认0）
高坚果：承受生命值（植物学挂钩）（30，默认为增加植物学0%生命值，每级+10%），最终爆炸概率（默认0，每级+3%，爆炸造成50%生命值）反伤伤害（默认反伤100%，每级+10%），反伤概率（默认概率10%，每级+4%） 最大生命值，护甲
三发豌豆：冷却时间， 基础伤害， 伤害百分比， 攻击检测范围，溅射伤害（升级增加百分比），子弹速度
火炬树桩： 豌豆增伤（每级+10%，可多个树桩叠加），豌豆溅射伤害（每级+10%），豌豆速度（每级+10%）阳光，肾上腺素，
坚果墙：基础伤害，百分比伤害，滚动速度，冷却时间，爆炸坚果概率（每级+3%,爆炸造成500%伤害），速度
寒冰菇：冰冻时间（默认3s，每级+0.5s），冷却时间，消耗阳光减少，冰冻期间攻速增加，幸运，生命恢复。
火爆辣椒：基础伤害，百分比伤害，冷却时间，转换阳光率（血量500%的百分之多少），肾上腺素，普通僵尸即死率，大型僵尸增伤，消耗阳光减少，爆炸范围
毁灭菇：基础伤害，百分比伤害，冷却时间，转换阳光率（血量500%的百分之多少），肾上腺素，普通僵尸即死率，大型僵尸增伤，消耗阳光减少
窝瓜：基础伤害，百分比伤害，冷却时间，转换阳光率（血量500%的百分之多少），普通僵尸即死率，大型僵尸增伤，连坐概率（每级+5%），消耗阳光减少
土豆雷：基础伤害，百分比伤害，冷却时间，转换阳光率（血量500%的百分之多少），肾上腺素，普通僵尸即死率，大型僵尸增伤，消耗阳光减少，爆炸范围，出土时间
玉米加农炮：基础伤害，百分比伤害，冷却时间，转换阳光率（血量500%的百分之多少），肾上腺素，普通僵尸即死率，大型僵尸增伤，爆炸范围

# 由于钻石和金币价值太高，所以金币价值变为25，钻石变为100
# 植物培养细节设计（暂定2023.4.3）：
# 培育的属性随机，最少设计5个，从中随机选三个
豌豆射手： 冷却时间， 基础伤害， 伤害百分比， 攻击检测范围，溅射伤害（升级增加百分比），子弹速度
双发豌豆由豌豆射手进化， 加特林豌豆由双发豌豆进化
# 2023.4.8-2023.4.14
大嘴花: 消化速度，攻击检测范围，一次性吞噬个数，转换金币率（血量200%的百分之多少），转换阳光率（血量500%的百分之多少），基础伤害（对巨人及僵王不能吞），百分比伤害。
仙人掌：穿透个数，冷却时间，基础伤害，百分比伤害，子弹速度，暴击伤害（默认20几率暴击）
三叶草：幸运，攻击速度（增加玩家攻击速度），风速，恢复（顺风增速，逆风恢复生命），风阻（僵尸逆风减速）  三叶草只能在最左边
香蒲：基础伤害，百分比伤害，穿透个数，暴击伤害（默认暴击率30），冷却时间，子弹速度，生命恢复（活血止痛之功效）
樱桃炸弹：基础伤害，百分比伤害，冷却时间，转换阳光率（血量500%的百分之多少），肾上腺素，普通僵尸即死率，大型僵尸增伤，消耗阳光减少，爆炸范围
咖啡豆：肾上腺素，幸运，植物学，范围，伤害，攻击速度，速度，力量  吃下减少（最大生命值，护甲）
玉米投手：最大生命值，基础伤害，百分比伤害，黄油概率（基础概率5%，每次增加3%），冷却时间，子弹速度，黄油控制时间。
曾哥、大喷菇：基础伤害，百分比伤害，攻击检测范围，冷却时间，暴击率（默认0，每级5%），暴击伤害（默认1.5，每级+0.1），伤害段数*2概率（默认0，每级3%）
吸金菇：金币，幸运，植物学，吸取金币个数（默认最多4个，每级+1），冷却时间，吸取持续时间
大蒜：生命恢复，肾上腺素，范围，植物学，最大生命值，攻击速度，速度，减少伤害10，减少幸运5；
墓碑吞噬者：僵尸数量减少概率（默认10，每级提升2，出僵尸时判断），玩家对僵尸增伤（算完基础伤害，百分比伤害后额外计算的，金币转换率，金币，伤害，护甲，冷却时间。
魅惑菇：基础伤害，百分比伤害，被魅惑者攻击次数（默认5次，只有发动攻击才能对僵尸造成伤害，碰撞不行，没次数了直接死亡，每级+2），连续触发概率（每级+3%，可连续触发），幸运。
磁力菇：金币，幸运，冷却时间，换取金币增加（默认0，每级+10），吸取铁制品个数（默认1个，每3级+1满级为5），吸取持续时间。
金盏花：金币，幸运，冷却时间，掉落金币概率（每级+3%，默认0），掉落双倍概率（每级+3，默认0），掉落钻石概率（每级+0.3%）
路灯：攻击检测范围，范围攻速提升增加（默认10%，每级+5%），范围生命恢复，范围伤害增加，攻击速度，范围
小喷菇：升级消耗的阳光变少（/5），且可无限成长 ： 基础伤害， 伤害百分比， 攻击检测范围，冷却时间，子弹速度，溅射伤害（升级增加百分比），发射子弹概率变多（每级+10%，超过100则不足100%部分再进行子弹判断），子弹大小；
南瓜：触发后恢复生命值提升（默认10%，每级+5%），护甲，最大生命值，生命恢复，攻击概率掉落南瓜（恢复10%生命值，每级+1%概率）
胆小菇：基础伤害， 伤害百分比， 攻击检测范围，冷却时间，子弹速度，溅射伤害（升级增加百分比），发射子弹概率变多（每级+10%，超过100则不足100%部分再进行子弹判断）
寒冰豌豆： 冷却时间， 基础伤害， 伤害百分比， 攻击检测范围，溅射伤害（升级增加百分比），子弹速度，减速百分比（默认20%每级+3%），减速持续时间（默认3s,每级+0.5s）
地刺，地刺王（可破坏载具）：基础伤害，百分比伤害，可破坏载具的数量（地刺王默认3，地刺默认1，破坏后该波消失，地刺王每3级+1，地刺每5级+1），减速百分比，减速持续时间，肾上腺素。
裂荚豌豆射手： 冷却时间， 基础伤害， 伤害百分比， 攻击检测范围，溅射伤害（升级增加百分比），子弹速度

# 增加道具显示以及属性悬浮提示功能（2023.4.8）
# 增加存档功能（2023.4.7）
存储道具列表，存储属性信息，存储植物信息，
继续游戏时读取存档，可选择重新开始或者删除存档，读档之间读取到商店界面。
在花园处增加波数显示，每波结束时自动存档
增加道具显示以及属性悬浮提示框再发视频
改键，增加

# 普通僵尸难度递增设计思路暂定为（2023.4.6）：
前4波敌人数值不变，为发育关卡，
第5到8波 ，敌人最大血量为基础血量 * 波数/3，伤害为基础伤害 * 波数/4，攻击触发范围 基础范围 + 基础范围*波数 / 10
第9到12波，敌人最大血量为基础血量 * 波数，伤害为基础伤害 * 波数/3
第13到16波，敌人最大血量为基础血量 * 波数 * 1.5，伤害为基础伤害 * 波数/1.5
第17到20波，敌人最大血量为基础血量 * 3*波数，伤害为基础伤害 * 波数
先版本无最终boss，20波以上统一设为 敌人最大血量为基础血量 * 5*波数，伤害为基础伤害 * 1.5*波数
移动速度为前4波不变，后面为 (波数 - 4 )*8, 最大120
僵尸生成数量设定

最大僵尸限定 80只，场上存在80只以上则不再生成

# 属性设计，道具购买于金币相关
- 最大生命值  12 +9 -3
- 生命恢复   11+7-4
- 肾上腺素（平底锅攻击概率回复一滴血） 14 +8 -6
- 力量 （平底锅以及道具的基础增加）14 +11-3
- 伤害 （伤害百分比增加）
- 攻击速度 （平底锅攻击速度）11 +8-3
- 范围  （平底锅及道具触发范围）11 +8 -3
- 暴击率  +10
- 速度 （增加移动速度） 11 +7-4
- 护甲： 伤害减免 13 +11-2
- 幸运：高品质道具及金币掉落概率 +14
- 阳光： 回合结束后增加阳光 + 5
- 金币： 回合结束后增加多少金币 +6
- 植物学 （植物伤害及效果等影响）+11


# 植物分为了四种类型，1随机种植战斗，2手动放置攻击，3培养增加属性，4功能型，如吸取金币，削弱敌人，阻挡敌人等
# 植物设计大致如下
- 仙人掌： 可以戳破该直线上的气球僵尸的气球， (穿透）（否则气球僵尸将是无敌的，只能逃跑）
- 三叶草： 随机往一个方向吹风，顺风跑时速度变快，逆风不受影响（气球僵尸逆风速度变慢）
- 香蒲： 需要水花盆，荷叶，+香蒲种子种植（全屏攻击，可刺破气球）
- 大嘴花：随机位置种植，吞噬小型僵尸，攻击巨人和僵王
- 咖啡豆： 购买种植出来后增加主角属性(属性确定后定)
- 玉米投手：同植物大战僵尸，往后统称同
- 花盆：所以植物培养都需要花盆，每次刷新赠送一个花盆，道具栏可刷新到水花盆
- 大喷菇：同
- 加特林豌豆：同
- 曾哥：同
- 金磁力菇：概率自动吸取掉落金币
- 大蒜：种植出来后增减某些属性
- 墓碑吞噬者：培养出来后，每次僵尸出场数-1
- 魅惑菇：解锁条件为死亡后不立即退出，僵尸自相残杀数量累计到达100解锁，魅惑第一只僵尸攻击场上其他僵尸
- 荷叶：种植香蒲必须，或者增加属性
- 磁力菇：可吸铁换钱
- 金盏花：产生金币,结束后额外产生
- 豌豆射手：同
- 路灯： 在灯附近僵尸移速变低，玩家攻速提高
- 小喷菇： 可用阳光无限进行成长
- 南瓜：主角免死一次
- 双发：同
- 胆小菇：同
- 寒冰豌豆:同
- 地刺王，地刺：都持续造成伤害，地磁扎一个车消失
- 双向豌豆：同
- 杨桃：同
- 向日葵：产生阳光，结束后额外产生
- 高坚果：同，具有嘲讽功能，在场时僵尸先攻击高坚果
- 三发：同
- 火炬：增加所有豌豆
- 双向日葵：大额阳光
- 坚果：从家里滚出碰撞，同小游戏坚果
- 
- 以下为手动植物
- 寒冰菇：手动植物
- 火爆辣椒：同，灰烬植物1
- 玉米加农炮： 两个玉米投手
- 樱桃炸弹： 手动点击释放的灰烬植物
- 毁灭菇：全屏造成伤害，会留下坑，主角不能踏入
- 窝瓜：手动点击释放
- 土豆雷：战斗中手动种植，只能炸一格
- 
- 所有植物都是随机种植，往一个方向发射，不能转向。


# 道具主要分为1，增加属性，2，解锁能力，3培养植物等
# 道具设计大致如下：
- 白色：
- 赊欠的下巴： 增加肾上腺素，生命恢复，攻击速度，最大生命值降低
- 哭泣的红心：增加最大生命值，减少肾上腺素
- 金属盘: 增加金币，护甲，最大生命值，减少肾上腺素
- 咽喉： 增加肾上腺素，最大生命值，减少生命恢复
- 小海菇帽：增加幸运，植物学，肾上腺素，降低最大生命值
- 阳光菇帽：增加阳光，最大生命值，降低力量
- 胡子：增加护甲，幸运,减少肾上腺素
- 眼镜：增加范围，暴击率
- 小胡子：增加幸运，暴击率
- 叶子：增加植物学，幸运
- 小花：增加最大生命值
- 肥料： 增加植物学
- 
- 蓝色：
- 钻石：金币增加，幸运增加
- 葵花：增加植物学，幸运，阳光
- 篮子：增加范围，暴击率，植物学，减少攻击速度
- 耙子：增加力量，暴击率
- 水壶：增加植物学，增加护甲，生命恢复，减低攻击速度
- 小丑盒子：增加幸运，肾上腺素，速度
- 水花：增加最大生命值，生命恢复，攻击速度，减少肾上腺素，护甲
- 草皮：增加生命恢复，移速，攻击速度，减少力量
- 地刺：增加速度，攻击速度，生命恢复，减少肾上腺素
- 蜗牛：增加护甲，植物学，降低速度,肾上腺素
- 除虫剂：增加植物学，增加肾上腺素，减少生命恢复
- 
- 紫色：
- 冰瓜：增加最大生命值，肾上腺素，范围，减少生命恢复
- 西瓜：增加最大生命值，生命恢复，范围,减少护甲
- 水花盆：增加植物学，解锁荷叶以及荷叶解锁香蒲。
- 金盏花瓣：增加金币，幸运，植物学
- 智慧树肥料：增加植物学，幸运，阳光
- 篮球：增加力量，攻击速度，暴击率，幸运
- 
- 菠菜：吃了我就大大力，增加力量，解锁飞头伤害，击退
- 小推车：增加力量，速度，每过一段时间对一条直线上的怪物造成伤害（大大力打死20只解锁）
- 
- 铁桶：增加护甲，力量，降低范围  （击败一只铁桶解锁）
- 路障：增加护甲，力量，伤害，降低范围（击败一只路障解锁）
- 铁门：增加护甲，力量，降低速度
- 报纸：增加植物学，金币，幸运，降低力量
- 扶梯：增加力量，暴击率，范围，减少攻击速度
- 橄榄球头盔：增加护甲，幸运，金币，降低范围
- 路牌：增加力量，范围，暴击率，护甲，降低速度
- 以上击败僵尸获取
- 
- 
- 红色：
- 冰碎：增加肾上腺素，幸运，金币，护甲，速度，减少生命恢复，攻击速度（使用寒冰菇10次解锁）
- 火焰：增加暴击率，阳光，伤害，速度，每秒扣血   （使用火爆辣椒10次解锁）
- 木槌：增加力量，暴击率，可以使用左键打击僵尸，频率受攻击速度影响  （打败巨人僵尸解锁）
- 传送门： 增加金币，幸运，伤害，在场上随机生成两个传送门，可进行两点传送，有冷却（大大力打死100只僵尸解锁）
- 上述道具获取可在游戏中显示
- 
- 雾机： 增加最大生命值，伤害，幸运，降低速度， 戴夫周围概率产生雾，使僵尸远程攻击命中率降低
- 灯光机:   增加阳光，增加攻击速度，
- 麦克风：肾上腺素，暴击率，
- 音响： 增加范围，力量，护甲
- 演唱会：凑齐灯光机，麦克风，音响，雾机，全属性增加，范围内僵尸持续受到伤害
- 
- 
- 后续可加特殊道具（2023.3.31）：
- 吸铁石，白色品质，降属性，获得鼠标自动吸附钱币效果，不需要点击  已完成2.23.4.4  -速度3
- 黑色方块，白色品质，降属性，获得鼠标自动吸附阳光效果，不需要点击  已完成2023.4.4 -4植物学
- 钥匙，白色品质，不加属性，免费刷新一次道具
- 小推车零件， 蓝色品质，不加属性。可增加小推车上限，  需要成就小推车一次攻击没打中一个敌人。
- 卡槽，蓝色品质，增加少量植物学，增加手动植物携带上限。
- 臭屁，紫色品质，减少属性，增加身后三个身位伤害效果


# 道具、属性与金币相关：
- 过第一关希望最多能买  3白，1蓝，0.5紫，0.2红  ，第一关怪物大概60只怪，掉落钱币概率为10 大概 60金币 可能掉金币，概率受幸运影响。 ，白色道具价格区间在10-30之间。  蓝色道具在45-80之间， 紫色道具在100- 140之间， 红色道具在280-320之间
- 每波过后道具价格膨胀率， 白色为 波数 / 3  蓝色为 波数 / 5  紫色为 波数 / 6  红色为 波数 / 8
- 植物价格始终不变
- 刷新价格每次（刷新次数 + 波数）*2+1，第一次刷新价格为 每波过后+2
- 
- 刷新道具出现的品质与幸运挂钩   出现白色品质的概率为100-蓝紫红  蓝色概率为 幸运40前 （20+幸运) 40-50(固定60) 50以上为（100-白色（10） - 紫色-红色）   紫色概率为幸运/2%  红色概率为幸运/10%   白色最低10 蓝色封顶60最低30  紫色概率封顶40， 红色概率封顶20
- 是否掉落银币，金币，钻石，与幸运挂钩，  掉落概率为  银币 （15 + 幸运）%  , 金币（幸运 / 6）%， 钻石（幸运 / 30）%
- 优先判断高品质，若每出则依次判断低品质
- 
- 生命值1：6金币
- 生命恢复1：5金币
- 肾上腺素1：3金币
- 力量1：15金币
- 伤害1：4金币
- 攻击速度1：5金币
- 范围1：3金币
- 暴击率1：10金币
- 速度 1：5金币
- 护甲1：10
- 幸运：1：8
- 阳光 25：10
- 金币1：2
- 植物学1：4
- 
- 蓝色总量金币优惠10
- 紫色优惠30
- 红色优惠50

# 植物价格设计（根据能力进行品质和价格划分）2023.4.2：
# 植物价格没有膨胀率， 白色 15-30，蓝色30-50， 紫色50-90， 红色 90-120
- 白色：
- 豌豆射手，  15
- 大嘴花，    20
- 大蒜,     20  
- 荷叶，  15
- 小喷菇，  15
- 胆小菇，  20
- 地刺，   20
- 向日葵，  20
- 土豆雷，  30
- 窝瓜，    30
- 坚果，  30
 
- 蓝色：
- 双发豌豆，  25  （豌豆射手进化）
- 仙人掌，   50  (穿透）
- 三叶草，   45
- 咖啡豆，  40
- 玉米投手，30
- 磁力菇，  35
- 金盏花，  50
- 南瓜，     50
- 寒冰豌豆，  40
- 高坚果，    40
- 双子向日葵，  50
- 寒冰菇，    50
- 樱桃炸弹，  50
- 大喷菇    40

- 紫色：
- 加特林豌豆，  35  （双发进化）
- 曾哥，    80
- 金磁力菇，  70
- 墓被吞噬者，  70 
- 魅惑菇，    90
- 路灯，    70
- 地刺王，   50
- 双向豌豆， 50 
- 杨桃，90
- 三发豌豆，50
- 火炬，90
- 玉米加农炮90

- 红色：
- 毁灭菇，120
- 辣椒100
- 香蒲，90

# 商店页面增加现有道具展示待做
# 属性悬浮展示提示信息待做。

# 阳光掉落：
掉落阳光数量1-6个不等，根据幸运决定数量， 幸运小于10掉1，到60最大，
幸运只影响掉落最大个数，掉落具体数目随机1-6个；

# 完全完成之后（僵尸，植物添加完毕）后续打算增加：
- 1.音乐会在下一波之前可由玩家随意选择音乐
- 2.玩家使用Json自由设计道具属性以及道具种类
- 3.道具购买，如音乐会，如果已购买某个组合道具，则该道具在组合完毕之前不会刷到(已完成2023.4.5，未：暂时还只有红色品质的音乐会可组合，后续白，蓝，紫各添加)
- 4.开始菜单点击冒险动画增加

