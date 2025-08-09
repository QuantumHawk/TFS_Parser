namespace TFS_Parser.Enums
{
    public enum TFE_Type
    {
        RAB = 1,
        RAB_FAKE = 12,
        RAB_POSL = 13,
        RAB_2par_AND = 2,
        RAB_2par_OR = 3,
        DIAGN = 4,
        DIAGN_2 = 5,
        RASV = 6,
        PROV_IF = 7,
        WHILE_DO = 8,
        WHILE_DO_2 = 9999,
        DO_UNTIL = 109999,
        DO_UNTIL_2 = 119999,
        DO_WHILE_DO = 9,
        DO_WHILE_DO_2 = 10,
        RASV_SIM = 11,
    }
}

//значения для типов блоков
// #define RAB         1  //рабоч
// #define RAB_FAKE       12  //рабоч фиктивная
// #define RAB_POSL       13  //послед рабоч
// #define RAB_2par_AND 2 // парал И
// #define RAB_2par_OR  3 // парал ИЛИ
// #define DIAGN       4  //  диагност РО-ДК
// #define DIAGN_2     5  //  функцио контр РО-ФК
// #define RASV        6  //  развилк ДК - "1 - РО1 "0 - РО2
// #define PROV_IF     7  //  контроль работосп (с предусловием) ДК "0 - РО
// #define WHILE_DO    8  // контр работ с постусловием РО - ДК "0 - РО
// #define WHILE_DO_2  9999  //   ФК с постусловием (не применяется)
// #define DO_UNTIL    109999 //  контроль Работспос как 7
// #define DO_UNTIL_2  119999 //     ФК как 5
// #define DO_WHILE_DO 9 //  ДК с востановл работосп РО1 ДК РО2
// #define DO_WHILE_DO_2 10 // ФК с доработкой РО1 - ФК - РО2
// #define RASV_SIM    11  // развилка для альтернативных подмножеств